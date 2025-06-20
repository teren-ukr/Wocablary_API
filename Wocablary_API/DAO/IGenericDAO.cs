using WocabWeb.API.Domain;

namespace WocabWeb.API.DAO
{
    public interface IGenericDAO<T> where T : EntityBase
    {
        //--------------------------------------- Основні методи
        Task<T> GetByIdAsync(long id);      // Отримати сутність за ID
        Task<IEnumerable<T>> GetAllAsync(); // Отримати всі сутності
        Task AddAsync(T entity);            // Додати нову сутність
        Task UpdateAsync(T entity);         // Оновити існуючу сутність
        Task DeleteAsync(long id);          // Видалити сутність за ID
        Task DeleteAsync(T entity);         // Видалити сутність за об'єктом

        //--------------------------------------- Додаткові методи
        Task<long> GetCountAsync();          // Отримати к-ть записів

    }
}
