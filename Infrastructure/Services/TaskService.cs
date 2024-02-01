using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;
//Service som hanterar TASK, CALENDAR och LOCATION
public class TaskService(CategoryRepository categoryRepository, TaskRepository taskRepository)
{
    //hämta min taskRepo
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly TaskRepository _taskRepository = taskRepository;


    // Metod för att skapa en ny uppgift genom att tillhandahålla information om Titel, beskrivning, deadline.
    //tar emot en instans av TaskDto
    public async Task<bool> CreateTaskAsync(TaskDto taskDto)
    {
        try
        {
            //Hämtar en kategori.
            var categoryEntity = _categoryRepository.GetAsync(x => x.CategoryName == taskDto.CategoryName);
            //Om kategorin är null, skapa ny categorin.
            if (categoryEntity == null)
            {
                categoryEntity = _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = taskDto.CategoryName });   
            }
            //Skapa en Task
            var taskEntity = new TaskEntity
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                Deadline = taskDto.Deadline,
                Status = taskDto.Status,
                CategoryId = categoryEntity.Id,
            };
            //Spara
           var result = await _taskRepository.CreateAsync(taskEntity);
           if (result != null)
            {
                return true;
            }
            
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;

    }

    public async Task<TaskEntity> GetTask(string title)
    {
        try
        {
            var result = await _taskRepository.GetAsync(x => x.Title == title);
            return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }


    //GET TASKS
    public async Task<IEnumerable<TaskDto>> GetTasks()
    {
        var tasks = new List<TaskDto>();

        try
        {
            var result = await _taskRepository.GetAllAsync(); //får tillbaka en taskentity
            //omvandla till en lista
            if (result != null)
            {
                foreach (var item in result)
                {
                    tasks.Add(new TaskDto
                    {
                        Title = item.Title,
                        Description = item.Description,
                        IsCompleted = item.IsCompleted,
                        Deadline = item.Deadline,
                        Status = item.Status,
                        CategoryName = item.Category.CategoryName
                    });
                    return tasks;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }




   // public async Task<TaskDto> UpdateTask(UpdatedTaskDto updatedTaskDto)
  //  {
      //  var updatedTask = await _taskRepository.UpdateAsync();
       // return updatedTask;
   // }







    //Metod för att hämta en uppgift



    //Metod för att ta bort en uppgift


    //metod för att markera en uppgift som slutförd


    //hämta en lista med alla uppgifter


    
}
