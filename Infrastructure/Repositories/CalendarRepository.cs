using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Infrastructure.Repositories;

public class CalendarRepository(DataContext context) : BaseRepository<CalendarEntity>(context)
{
    private readonly DataContext _context = context;


    public override async Task<CalendarEntity> GetAsync(Expression<Func<CalendarEntity, bool>> expression)
    {
        try
        {
            var existingEntity = await _context.Calendars
                .Include(i => i.Tasks)
                .FirstOrDefaultAsync(expression);

            if (existingEntity != null)
            {
                return existingEntity;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<IEnumerable<CalendarEntity>> GetAllAsync()
    {
        try
        {
            //hitta entiteten
            var existingEntities = await _context.Calendars
                .Include(i => i.Tasks)
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

