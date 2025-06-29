using System.Reflection;
using HomeApi.Configuration;
using Microsoft.OpenApi.Models;
using HomeApi.Contracts.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ��������� ��������� ������-����
builder.Configuration.AddJsonFile("HomeOptions.json", optional: false, reloadOnChange: true);

// ������������ ������������
builder.Services.Configure<HomeOptions>(builder.Configuration.GetSection("HomeOptions"));
builder.Services.Configure<HomeOptions>(opt =>
{
    // ������ ���������� ���������� ��������
    opt.Area = 120;

    // ����� �������� �������������� ������:
    // opt.FloorAmount = CalculateFloors();
    // opt.GasConnected = Environment.IsDevelopment();
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

// ������ �������
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
