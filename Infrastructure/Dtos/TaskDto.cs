namespace Infrastructure.Dtos;

public record TaskDto (string Title, string Description, DateTime Deadline, string Status, string CategoryName);