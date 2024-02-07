using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Presentation;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\EC\SQL\TodoApp\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddScoped<CategoryRepository>();
    services.AddScoped<CategoryService>();
    services.AddScoped<TaskRepository>();
    services.AddScoped<TaskService>();
    services.AddScoped<UserRepository>();
    services.AddScoped<UserService>();
    services.AddScoped<CalendarRepository>();
    services.AddScoped<CalendarService>();
    services.AddScoped<LocationRepository>();
    services.AddScoped<LocationService>();

    services.AddSingleton<MenuService>();


}).Build();

var menuService = builder.Services.GetRequiredService<MenuService>();

//menuService.CreateTask_UI();
await menuService.GetTasks_UI();


