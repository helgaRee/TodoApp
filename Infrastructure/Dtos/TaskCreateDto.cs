namespace Infrastructure.Dtos;

public class TaskCreateDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public bool IsPrivate { get; set; } = false!;
        public DateTime? Time {  get; set; }   
        public DateTime? Date { get; set; }
        public DateTime? Year { get; set; }
        public string LocationName { get; set; } = null!;
        public string StreetName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string UserName { get; set; } = null!;
        public string? Email {  get; set; } 
        public string? Password { get; set; }
}