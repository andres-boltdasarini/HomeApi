using System.Reflection;
using HomeApi.Configuration;
using Microsoft.OpenApi.Models;
using HomeApi.Contracts.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем кастомный конфиг-файл
builder.Configuration.AddJsonFile("HomeOptions.json", optional: false, reloadOnChange: true);

// Регистрируем конфигурацию
builder.Services.Configure<HomeOptions>(builder.Configuration.GetSection("HomeOptions"));
builder.Services.Configure<HomeOptions>(opt =>
{
    // Ручная перезапись конкретных значений
    opt.Area = 120;

    // Можно добавить дополнительную логику:
    // opt.FloorAmount = CalculateFloors();
    // opt.GasConnected = Environment.IsDevelopment();
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

// Другие сервисы
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
