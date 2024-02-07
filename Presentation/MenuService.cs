using Infrastructure.Dtos;
using Infrastructure.Services;

namespace Presentation;

public class MenuService
{
    private readonly TaskService _taskService;

    public MenuService(TaskService taskService)
    {
        _taskService = taskService;
    }

    public void CreateTask_UI()
    {
        Console.Clear();
        Console.WriteLine("-----CREATE TASK------");

        Console.Write("Task title: ");
        string title = Console.ReadLine()!;

        Console.Write("description: ");
        string description = Console.ReadLine()!;
   

        Console.Write("status ");
        string status = Console.ReadLine()!;

        Console.Write("Category: ");
        string categoryName = Console.ReadLine()!;

        Console.Write("Location: ");
        string locationName = Console.ReadLine()!;

        TaskCreateDto taskCreateDto = new()
        {
            Title = title,
            Description = description,
            Status = status,
            CategoryName = categoryName,
            LocationName = locationName
        };
        if (taskCreateDto != null)
        {
            Console.Clear();
            Console.WriteLine("Task was created");
            Console.ReadKey();
        }


    }

    public async Task GetTasks_UI()
    {
        Console.Clear();
        var tasks = await _taskService.GetTasksAsync();
        foreach(var task in tasks)
        {
            Console.WriteLine($"{task.Title} - {task.Description}");
        }
        Console.ReadKey();
    }

    public async Task UpdateTask_UI()
    {
        Console.Clear();
        Console.Write("Enter Task Id: ");
        var id = int.Parse(Console.ReadLine()!);

        var task = await _taskService.GetTaskAsync(id);
        if(task != null)
        {
            Console.WriteLine($"{task.title} - {task.description}");
            Console.WriteLine();

            Console.Write("New task title: ");
             task.title = Console.ReadLine()!;

            Console.Write("New task desctiption: ");
            task.description = Console.ReadLine()!;

  

            Console.Write("Set deadline: ");
            task.deadline = Console.ReadLine()!;

            Console.Write("New status: ");
            task.status = Console.ReadLine();
        }
    }
}
