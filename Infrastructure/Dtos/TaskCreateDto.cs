namespace Infrastructure.Dtos;

public record TaskCreateDto (string Title, string Description, DateTime Deadline, string Status, string CategoryName);