using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class CalendarService(CalendarRepository calendarRepository)
{
    private readonly CalendarRepository _calendarRepository = calendarRepository;

    //skapa en calenderEntity om den inte redan finns
    public async Task<CalendarEntity> CreateCalendarAsync(CalendarDto dto)
    {
        try
        {
            if (!await _calendarRepository.ExistingAsync(x => x.CalendarId == dto.CalendarId))
            {
                //skapar och mappar in kalendern
                var calendarEntity = new CalendarEntity 
                { 
                    Time = dto.Time,
                    Date = dto.Date,
                    Year = dto.Year,
                };

                //lägg till kalendern i databasen
                return await _calendarRepository.CreateAsync(calendarEntity);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    //HÄMTA en kalender
    public async Task<CalendarDto> GetCalendarAsync(Expression<Func<CalendarEntity, bool>> predicate)
    {
        try
        {
            var calendarEntity = await _calendarRepository.GetAsync(predicate);
            if (calendarEntity != null)
            {
                var calendarDto = new CalendarDto(
                    calendarEntity.CalendarId,
                    calendarEntity.Time!.Value,
                    calendarEntity.Date!.Value,
                    calendarEntity.Year!.Value
                );
                return calendarDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return null!;
    }

    public async Task<CalendarEntity> GetCalendarByIdAsync(CalendarDto dto)
    {
        try
        {
            var calendarEntity = await _calendarRepository.GetAsync(x => x.CalendarId == dto.CalendarId);
            if (calendarEntity != null)
            {
                return calendarEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return null!;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns a list of CalendarDtos.</returns>
    public async Task<IEnumerable<CalendarDto>> GetCalendarsAsync()
    {
        try
        {
            var calendarEntities = await _calendarRepository.GetAllAsync();

            if (calendarEntities != null)
            {
                var list = new List<CalendarDto>();
                foreach (var calendarEntity in calendarEntities)
                    list.Add(new CalendarDto
                    (
                        calendarEntity.CalendarId, 
                        calendarEntity.Time!.Value, 
                        calendarEntity.Date!.Value, 
                        calendarEntity.Year!.Value
                    ));

                return list;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return null!;
    }

    //uppdatera calendar
    public async Task<bool> UpdateCalendar(CalendarDto dto)
    {
        //skapa en ny calender för uppdatering
        var updatedCalendar = new CalendarEntity
        {
            Date = dto.Date,
            Time = dto.Time,
            Year = dto.Year,
        };
        //spara in den uppdaterade informationen till usern
        var newCalendar = await _calendarRepository.UpdateAsync(x => x.CalendarId == dto.CalendarId, updatedCalendar);
        //skapa en ny DTO med den uppdaterade usern
        if(newCalendar != null)
        {
            var calendarDto = new CalendarDto
                (
                    newCalendar.CalendarId,
                    newCalendar.Date!.Value,
                    newCalendar.Time!.Value,
                    newCalendar.Year!.Value
                );
            return calendarDto != null;
        }
        return false;
    }

    public async Task<bool> DeleteCalendar(CalendarEntity entity)
    {
        try
        {
            var deletedCalendar = await _calendarRepository.DeleteAsync(x => x.CalendarId == entity.CalendarId);
            if(deletedCalendar)          
               return true;         
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + (ex.Message)); }
        return false;
    }

}
