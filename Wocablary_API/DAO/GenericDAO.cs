using NHibernate;
using NHibernate.Linq;
using WocabWeb.API.Domain;

using ISession = NHibernate.ISession; // Посилання на EntityBase

namespace WocabWeb.API.DAO
{
    public class GenericDAO<T> : IGenericDAO<T> where T : EntityBase
    {
        // ISessionFactory створюється один раз на старті програми
        // ISession створюється для кожного запиту/одиниці роботи
        protected readonly ISession _session;


        // Конструктор, через який буде інжектовано ISession
        public GenericDAO(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }


        public virtual async Task<T> GetByIdAsync(long id)
        {
            // Get<T>() намагається завантажити об'єкт. Якщо не знайдено, повертає null.
            // Load<T>() повертає проксі-об'єкт. Якщо об'єкт не існує, кине ObjectNotFoundException при першому доступі.
            // Для простих GetById краще Get<T>().
            return await _session.GetAsync<T>(id);

        }


        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            // Створюємо критерій для отримання всіх об'єктів типу T
            // або використовуємо LINQ to NHibernate (.Query<T>().ToListAsync())
            return await _session.Query<T>().ToListAsync();
        }


        public virtual async Task AddAsync(T entity)
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                try
                {
                    await _session.SaveAsync(entity); // Зберігає новий об'єкт
                    await transaction.CommitAsync();  // Підтверджує транзакцію
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(); // Відкат транзакції у разі помилки
                    throw; //перекидає виняток далі
                }
            }
        }



        //=====================================================================//



        public virtual async Task UpdateAsync(T entity)
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                try
                {
                    // Update() перевіряє, чи об'єкт вже асоційований з сесією.
                    // Якщо асоційований, його стан буде оновлено при коміті.
                    // Якщо не асоційований (наприклад, завантажений в іншій сесії),
                    // він буде асоційований, а його стан оновлено в БД.
                    // Merge() також можна використовувати, особливо для об'єктів,
                    // які могли бути змінені поза сесією.
                    await _session.UpdateAsync(entity);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public virtual async Task DeleteAsync(long id)
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                try
                {
                    // Спочатку отримуємо об'єкт за ID, потім видаляємо його.
                    // Це гарантує, що об'єкт існує перед видаленням.
                    var entityToDelete = await _session.GetAsync<T>(id);

                    if (entityToDelete != null)
                    {
                        await _session.DeleteAsync(entityToDelete);
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        throw new ArgumentException($"Entity with ID {id} not found.");
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                try
                {
                    await _session.DeleteAsync(entity);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        public virtual async Task<long> GetCountAsync()
        {
            return await _session.QueryOver<T>().RowCountInt64Async();
        }



    }
}
