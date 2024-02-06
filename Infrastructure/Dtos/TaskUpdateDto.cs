namespace Infrastructure.Dtos;

public record TaskUpdateDto (int TaskId, string Title, string Description, bool IsCompleted, DateTime Deadline, string Status);