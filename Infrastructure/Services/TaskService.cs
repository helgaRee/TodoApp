using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Infrastructure.Services;
//Service som hanterar TASK, CALENDAR och LOCATION
public class TaskService
{
    private readonly LocationRepository _locationRepository;
    private readonly CalendarRepository _calendarRepository;
    private readonly UserRepository _userRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly TaskRepository _taskRepository;

    public TaskService(LocationRepository locationRepository, CalendarRepository calendarRepository,
        UserRepository userRepository, CategoryRepository categoryRepository, TaskRepository taskRepository)
    {
        _locationRepository = locationRepository;
        _calendarRepository = calendarRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _taskRepository = taskRepository;
    }
    /// <summary>
    /// Creates a new Category if it doesnt already exist. Then creates a new Task.
    /// </summary>
    /// <param name="taskDto"></param>
    /// <returns>Returns a new TaskEntity with a category, or null if nothing was created.</returns>
    public async Task<bool> CreateTaskAsync(TaskCreateDto taskDto)
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

                // Kontrollera att alla nödvändiga entiteter skapades korrekt innan du skapar taskEntity
                if (locationEntity == null || calendarEntity == null || userEntity == null || categoryEntity == null)
                {
                    Debug.WriteLine("En eller flera entiteter kunde inte skapas.");
                    return false;
                }

                // Skapa taskEntity och associera med Location, Calendar, User och Category
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
              

                return true;
            }
            else
            {
                // Om uppgiften redan finns i databasen, returnera null eller kasta ett undantag
                return false;
            }
        }

        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        // Skapa den nya uppgiften och använd den befintliga eller nyss skapade kategorins ID
        return false;
    }



    /// <summary>
    /// Gets a task from the database
    /// </summary>
    /// <param name="title"></param>
    /// <returns>An existing task from the database, or null if the database is empty</returns>
    public async Task<TaskEntity> GetTaskAsync(string title)
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
    public async Task<IEnumerable<TaskEntity>> GetTasksAsync()
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
    public async Task<bool> UpdateTaskAsync(TaskUpdateDto dto)
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
            var updatedTask = await _taskRepository.UpdateAsync(x => x.TaskId == dto.TaskId, taskEntity);

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
    public async Task<bool> DeleteTaskAsync(int id)
    {
        //hämta
        var deletedTask = await _taskRepository.DeleteAsync(x => x.TaskId == id);
        return true;
        
    }

    public async Task<bool> IsCompleteAsync(int id)
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







