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
    services.AddSingleton<CalendarRepository>();

}).Build();

builder.Start();

Console.ReadKey();
Console.Clear();

var taskService = builder.Services.GetRequiredService<TaskService>();
var taskDto = new TaskCreateDto
{
    Title = "A1 Title",
    Description = "a1 Description",
    Deadline = DateTime.Now,
    Status = "A1 ongoing",
    CategoryName = "Category-Test",
};

var taskEntity = await taskService.CreateTaskAsync(taskDto); // Skapar en ny uppgift

if (taskEntity != null)
{
    var categoryId = taskEntity.CategoryId; // Hämtar kategorins ID från den nya uppgiften
    // Använd categoryId på något sätt...
}
else
{
    Console.WriteLine("Something went wrong creating the task.");
}

Console.ReadKey();