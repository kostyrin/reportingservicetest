using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using ReportService.Data.Abstractions;
using ReportService.Data.Services;
using System.Data;
using System;
using ReportService.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(builder.Configuration.GetConnectionString("EmployeeDB")));
builder.Services.AddScoped<IReportingRepository, ReportingRepository>();
builder.Services.AddScoped<IReportingService, ReportingService>();
builder.Services.AddHttpClient<BuhApiClient>(cfg => cfg.BaseAddress = new Uri(builder.Configuration.GetValue<string>("buhApiUrl")));
builder.Services.AddHttpClient<SalaryApiClient>(cfg => cfg.BaseAddress = new Uri(builder.Configuration.GetValue<string>("salaryApiUrl")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
