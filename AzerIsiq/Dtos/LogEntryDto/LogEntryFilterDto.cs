namespace AzerIsiq.Dtos.LogEntryDto;

public class LogEntryFilterDto
{
    public string? EntryName { get; set; }
    public string? UserRole { get; set; }
    public string? Action { get; set; }
    public string? UserNameSearch { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}