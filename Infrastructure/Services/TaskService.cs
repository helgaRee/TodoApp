using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Infrastructure.Services;
//Service som hanterar TASK, CALENDAR och LOCATION
public class TaskService(UserRepository userRepository, LocationRepository locationRepository, CategoryRepository categoryRepository, TaskRepository taskRepository, CalendarRepository calendarRepository)
{
    //hämta min taskRepo
    private readonly TaskRepository _taskRepository = taskRepository;
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    //private readonly CategoryService _categoryService = categoryService;
    //private readonly CalendarService _calendarService = calendarService;


    private readonly LocationRepository _locationRepository = locationRepository;
    private readonly CalendarRepository _calendarRepository = calendarRepository;
    private readonly UserRepository _userRepository = userRepository;

    /// <summary>
    /// Creates a new Category if it doesnt already exist. Then creates a new Task.
    /// </summary>
    /// <param name="taskDto"></param>
    /// <returns>Returns a new TaskEntity with a category, or null if nothing was created.</returns>
    public async Task<TaskDto> CreateTaskAsync(TaskCreateDto taskDto)
    {
        try
        {

            // Kontrollera om kategorin redan existerar i databasen - SÖKNING och HÄMTAR
            var taskInDatabase = await _taskRepository.GetAsync(x => x.Title == taskDto.Title);

            if (taskInDatabase == null)
            {
                // Skapa Location
                var locationEntity = await _locationRepository.CreateAsync(new LocationEntity
                {
                    LocationName = taskDto.LocationName,
                    StreetName = taskDto.StreetName,
                    PostalCode = taskDto.PostalCode,
                    City = taskDto.City
                });

                // Skapa Calendar
                var calendarEntity = await _calendarRepository.CreateAsync(new CalendarEntity
                {
                    Time = taskDto.Time,
                    Date = taskDto.Date,
                    Year = taskDto.Year
                });

                // Skapa User
                var userEntity = await _userRepository.CreateAsync(new UserEntity
                {
                    UserName = taskDto.UserName,
                    Email = taskDto.Email!,
                    Password = taskDto.Password!
                });

                // Skapa Category
                var categoryEntity = await _categoryRepository.CreateAsync(new CategoryEntity
                {
                    CategoryName = taskDto.CategoryName,
                    IsPrivate = taskDto.IsPrivate
                });

                // Skapa TaskEntity och associera med Location, Calendar, User och Category
                var taskEntity = new TaskEntity
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    Deadline = taskDto.Deadline,
                    Status = taskDto.Status,
                    LocationId = locationEntity.LocationId,
                    CalendarId = calendarEntity.CalendarId,
                    UserId = userEntity.UserId,
                    CategoryId = categoryEntity.CategoryId
                };

                // Spara uppgiften i databasen
                var createdTaskEntity = await _taskRepository.CreateAsync(taskEntity);

                // Skapa och returnera DTO för den skapade uppgiften
                var createdTaskDto = new TaskDto
                {
                    Title = createdTaskEntity.Title,
                    Description = createdTaskEntity.Description,
                    Deadline = createdTaskEntity.Deadline.Value,
                    Status = createdTaskEntity.Status,
                    // Fyll i med resten av egenskaperna om det behövs
                };

                return createdTaskDto;
            }
            else
            {
                // Om uppgiften redan finns i databasen, returnera null eller kasta ett undantag
                return null!;
            }
        }


        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        // Skapa den nya uppgiften och använd den befintliga eller nyss skapade kategorins ID
        return null!;
    }



    /// <summary>
    /// Gets a task from the database
    /// </summary>
    /// <param name="title"></param>
    /// <returns>An existing task from the database, or null if the database is empty</returns>
    public async Task<TaskEntity> GetTask(string title)
    {
        try
        {
            var taskEntity = await _taskRepository.GetAsync(x => x.Title == title);
            return taskEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns>Returns a list of existing tasks or an empty list.</returns>
    public async Task<IEnumerable<TaskEntity>> GetTasks()
    {
        //var tasks = new List<TaskCreateDto>();
        try
        {
           var tasks = await _taskRepository.GetAllAsync();
            return tasks;         
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return Enumerable.Empty<TaskEntity>();    
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<bool> UpdateTask(TaskUpdateDto dto)
    {

        //SKAPA EN TASK - UPPDATERA EN TASK
        var taskEntity = new TaskEntity
        {
            TaskId = dto.TaskId,
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            Deadline = dto.Deadline,
            Status = dto.Status,
        };

        //Skapa en ny instans av TaskEntity och stoppa in värdena från Taskentit´teten som hämtas från Dto.
        //Den nya Tasken ska uppdateras med värden från Dto. "skapar en kopia av entiteten och tjoffar in värden"

            //Asynkront anrop till metoden UpdateAsync.  Den tar 2 parametrar, ett lambdauttryck för att filtrera vilken kategori som ska uppdateras
            //baserat på CategoryId, samt den nya kategorin som ska uppdateras
            var updatedTask = await _taskRepository.UpdateAsync(x => x.Title == dto.Title, taskEntity);

            //OM task inte är null, skapa en ny TaskDto med de uppdaterade värdena. 
            if (updatedTask != null)
            {
                var taskDto = new TaskUpdateDto(
                    updatedTask.TaskId,
                    updatedTask.Title,
                    updatedTask.Description!,
                    updatedTask.IsCompleted!.Value,
                    updatedTask.Deadline!.Value,
                    updatedTask.Status!
                );
            return true;
            }
            return false;
    }


  /// <summary>
  /// Deletes an existing task by Id.
  /// </summary>
  /// <param name="id"></param>
  /// <returns>Returns true if deletion was succesful, else false.</returns>
    public async Task<bool> DeleteTask(int id)
    {
        //hämta
        var deletedTask = await _taskRepository.DeleteAsync(x => x.TaskId == id);
        return true;
        
    }

    public async Task<bool> IsComplete(int id)
    {
        //hämta
        var completedTask = await _taskRepository.GetAsync(x => x.TaskId == id);
        if(completedTask != null)
        {
            //ska kunna bocka av denna att den nu är färdig.
        }
        return true;

    }






}











    //metod för att markera en uppgift som slutförd







