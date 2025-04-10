namespace AzerIsiq.Dtos.LogEntryDto;

public class LogEntryCreateDto
{
    public string Action { get; set; }
    public string EntityName { get; set; }
    public int EntityId { get; set; }
    public int UserId { get; set; }
}