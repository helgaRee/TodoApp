using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class TaskRepository(DataContext context) : BaseRepository<TaskEntity>(context)
{
  //  private readonly DataContext _context = context;

    public override async Task<TaskEntity> GetAsync(Expression<Func<TaskEntity, bool>> expression)
    {
        try
        {
            var existingEntity = await context.Tasks
                .Include(i => i.Location)
                .Include(i => i.Calendar)
                .Include(i => i.User)//.ThenInclude(i => i.Tasks).ThenInclude(i => i.IsCompleted)
                .Include (i => i.Category)//.ThenInclude(i => i.Tasks).ThenInclude(i => i.Deadline)
                .FirstOrDefaultAsync(expression);

            if (existingEntity != null)
            {
                return existingEntity;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<IEnumerable<TaskEntity>> GetAllAsync()
    {
        try
        {
            //hitta entiteten
            var existingEntities = await context.Tasks
                .Include(i => i.Location)
                .Include(i => i.Calendar)
                .Include(i => i.User)//.ThenInclude(i => i.Tasks).ThenInclude(i => i.IsCompleted)
                .Include(i => i.Category)//.ThenInclude(i => i.Tasks).ThenInclude(i => i.Deadline)
                .ToListAsync();
            //returnera om inte null
            if (existingEntities != null)
            {
                return existingEntities;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}

