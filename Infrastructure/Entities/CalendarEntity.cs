using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class CalendarEntity
{
    [Key]
    [Column(TypeName = "int")]
    public int CalendarId { get; set; }

    [Column(TypeName = "DateTime")]
    public DateTime Time { get; set; }

    [Column(TypeName = "DateTime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "DateTime")]
    public DateTime Year { get; set; }

    public virtual ICollection<TaskEntity> Tasks { get; set; } = new HashSet<TaskEntity>();


}
