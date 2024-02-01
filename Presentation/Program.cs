using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\EC\SQL\TodoApp\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddSingleton<CategoryRepository>();
    services.AddSingleton<TaskRepository>();
    services.AddSingleton<UserRepository>();
    services.AddSingleton<TaskService>();

}).Build();

builder.Start();

Console.ReadKey();

Console.Clear();
var taskService = builder.Services.GetRequiredService<TaskService>();
var result = await taskService.CreateTaskAsync(new TaskDto
{
    Title = "A1 Title",
    Description = "a1 Description",
    IsCompleted = true,
    Status = "A1 ongoing",
    CategoryName = "Category-Test",

});

if (result)
    Console.WriteLine("Lyckades!");
else Console.WriteLine("Något gick fel..");
Console.ReadKey();