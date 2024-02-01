namespace Infrastructure.Dtos;

public record TaskDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime Deadline { get; set; }
    public string? Status { get; set; }
    public string CategoryName { get; set; } = null!;
}
