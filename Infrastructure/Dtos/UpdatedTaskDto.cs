namespace Infrastructure.Dtos;

public class UpdatedTaskDto
{
public bool IsCompleted { get; set; } = false;

public DateTime Deadline { get; set; }
public string? Status { get; set; }

}

