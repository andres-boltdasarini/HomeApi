using System.Reflection;
using FluentValidation.AspNetCore;
using HomeApi;
using HomeApi.Configuration;
using HomeApi.Contracts.Validation;
using HomeApi.Data;
using HomeApi.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Загрузка конфигурации
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile("HomeOptions.json");

// Добавление сервисов
var services = builder.Services;

// Подключаем автомаппинг
var assembly = Assembly.GetAssembly(typeof(MappingProfile));
services.AddAutoMapper(assembly);

// Регистрация репозиториев
services.AddSingleton<IDeviceRepository, DeviceRepository>();
services.AddSingleton<IRoomRepository, RoomRepository>();

// Настройка базы данных
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

// Подключаем валидацию
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

// Конфигурация опций
services.Configure<HomeOptions>(builder.Configuration);
services.Configure<Address>(builder.Configuration.GetSection("Address"));

// Добавляем контроллеры
services.AddControllers();

// Настройка Swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" });
});

var app = builder.Build();

// Конфигурация pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();