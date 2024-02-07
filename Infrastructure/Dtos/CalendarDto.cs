using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public record CalendarDto(int CalendarId, DateTime Time, DateTime Date, DateTime Year);


