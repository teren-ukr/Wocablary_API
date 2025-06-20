using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; // Додайте цей using

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg; // Для об'єкта Configuration


// Using-и для DAO та Domain об'єктів
using WocabWeb.API.Domain; // Для WordMap, TagMap та EntityBase
using WocabWeb.API.DAO;




//------------------------------------------------------------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Замість builder.Services.AddOpenApi(); (якщо використовується Swashbuckle, а не вбудований OpenApi)
builder.Services.AddEndpointsApiExplorer(); // Додає сервіси для OpenApi
builder.Services.AddSwaggerGen(); // Додає генерацію Swagger/OpenAPI специфікації



//------------------------------------------------------------------------------------------------------------
// --- Конфігурація NHibernate ---
// Отримання рядка підключення з appsettings.json
var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'PostgreSqlConnection' not found in appsettings.json.");
}


//------------------------------------------------------------------------------------------------------------
builder.Services.AddSingleton<ISessionFactory>(factory =>
{
    var config = Fluently.Configure()
        .Database(PostgreSQLConfiguration.Standard.ConnectionString(connectionString)
            .ShowSql() // Для розробки
            .FormatSql() // Для розробки
        )

        // Додавання мапінгів з асамблеї, де знаходяться *.Map.cs файли    
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<WordMap>()) // Припускаючи, що WordMap знаходиться в WocabWeb.API.Domain
        .BuildConfiguration(); // Будуємо об'єкт Configuration

    // Створення або оновлення схеми БД при запуску
    // Цей блок краще використовувати тільки на етапі розробки
    if (builder.Environment.IsDevelopment())
    {
        // Перевіряємо, чи база даних порожня, і створюємо її, або оновлюємо
        // Цей код може бути не ідеальним для production, але підійде для розробки
        var schemaUpdate = new SchemaUpdate(config);
        schemaUpdate.Execute(false, true); // false: не виводити SQL в консоль; true: виконати в БД

        // Альтернативно, для чистої схеми кожен раз (ОБЕРЕЖНО, ВИДАЛЯЄ ВСІ ДАНІ!)
        // new SchemaExport(config).Create(false, true); 
    }

    return config.BuildSessionFactory();
});


//------------------------------------------------------------------------------------------------------------
// Реєстрація ISession у контейнері DI з життєвим циклом Scoped
// Це гарантує, що для кожного HTTP-запиту буде створено нову сесію
builder.Services.AddScoped<NHibernate.ISession>(factory =>
{
    var sessionFactory = factory.GetRequiredService<ISessionFactory>();
    return sessionFactory.OpenSession();
});


//------------------------------------------------------------------------------------------------------------
// --- Реєстрація DAO/репозиторіїв ---
// Реєстрація GenericDAO
builder.Services.AddScoped(typeof(IGenericDAO<>), typeof(GenericDAO<>));

// Реєстрація специфічних DAO
builder.Services.AddScoped<IWordDAO, WordDAO>();
builder.Services.AddScoped<ITagDAO, TagDAO>();
// ... інші специфічні DAO


//------------------------------------------------------------------------------------------------------------
// Налаштування CORS (важливо для React frontend, який працюватиме на іншому домені/порті)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        corsBuilder =>
        {
            corsBuilder.WithOrigins("http://localhost:3000") // Замініть на URL вашого React-додатка
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            // .AllowCredentials(); // Якщо ви використовуєте куки або авторизаційні заголовки
        });
});




//------------------------------------------------------------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.MapOpenApi(); // Це для мінімальних API, якщо використовуєте Swashbuckle, то не потрібно
}

app.UseHttpsRedirection();

app.UseRouting(); // Додайте UseRouting для коректної роботи CORS та маршрутизації

app.UseCors("AllowReactApp"); // Застосовуйте CORS політику ПІСЛЯ UseRouting і ПЕРЕД UseAuthorization

app.UseAuthorization();

app.MapControllers(); // Мапує атрибути контролерів

app.Run();