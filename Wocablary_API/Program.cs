using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; // ������� ��� using

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg; // ��� ��'���� Configuration


// Using-� ��� DAO �� Domain ��'����
using WocabWeb.API.Domain; // ��� WordMap, TagMap �� EntityBase
using WocabWeb.API.DAO;




//------------------------------------------------------------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ������ builder.Services.AddOpenApi(); (���� ��������������� Swashbuckle, � �� ���������� OpenApi)
builder.Services.AddEndpointsApiExplorer(); // ���� ������ ��� OpenApi
builder.Services.AddSwaggerGen(); // ���� ��������� Swagger/OpenAPI ������������



//------------------------------------------------------------------------------------------------------------
// --- ������������ NHibernate ---
// ��������� ����� ���������� � appsettings.json
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
            .ShowSql() // ��� ��������
            .FormatSql() // ��� ��������
        )

        // ��������� ������ � �������, �� ����������� *.Map.cs �����    
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<WordMap>()) // �����������, �� WordMap ����������� � WocabWeb.API.Domain
        .BuildConfiguration(); // ������ ��'��� Configuration

    // ��������� ��� ��������� ����� �� ��� �������
    // ��� ���� ����� ��������������� ����� �� ���� ��������
    if (builder.Environment.IsDevelopment())
    {
        // ����������, �� ���� ����� �������, � ��������� ��, ��� ���������
        // ��� ��� ���� ���� �� ��������� ��� production, ��� ����� ��� ��������
        var schemaUpdate = new SchemaUpdate(config);
        schemaUpdate.Execute(false, true); // false: �� �������� SQL � �������; true: �������� � ��

        // �������������, ��� ����� ����� ����� ��� (��������, �����ߪ �Ѳ ��Ͳ!)
        // new SchemaExport(config).Create(false, true); 
    }

    return config.BuildSessionFactory();
});


//------------------------------------------------------------------------------------------------------------
// ��������� ISession � ��������� DI � ������� ������ Scoped
// �� �������, �� ��� ������� HTTP-������ ���� �������� ���� ����
builder.Services.AddScoped<NHibernate.ISession>(factory =>
{
    var sessionFactory = factory.GetRequiredService<ISessionFactory>();
    return sessionFactory.OpenSession();
});


//------------------------------------------------------------------------------------------------------------
// --- ��������� DAO/���������� ---
// ��������� GenericDAO
builder.Services.AddScoped(typeof(IGenericDAO<>), typeof(GenericDAO<>));

// ��������� ����������� DAO
builder.Services.AddScoped<IWordDAO, WordDAO>();
builder.Services.AddScoped<ITagDAO, TagDAO>();
// ... ���� ��������� DAO


//------------------------------------------------------------------------------------------------------------
// ������������ CORS (������� ��� React frontend, ���� ����������� �� ������ �����/����)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        corsBuilder =>
        {
            corsBuilder.WithOrigins("http://localhost:3000") // ������ �� URL ������ React-�������
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            // .AllowCredentials(); // ���� �� ������������� ���� ��� ������������ ���������
        });
});




//------------------------------------------------------------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.MapOpenApi(); // �� ��� ��������� API, ���� ������������� Swashbuckle, �� �� �������
}

app.UseHttpsRedirection();

app.UseRouting(); // ������� UseRouting ��� �������� ������ CORS �� �������������

app.UseCors("AllowReactApp"); // ������������ CORS ������� ϲ��� UseRouting � ����� UseAuthorization

app.UseAuthorization();

app.MapControllers(); // ���� �������� ����������

app.Run();