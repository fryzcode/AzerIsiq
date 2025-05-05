using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Dtos.ElectronicAppealDto;

public class ElectronicAppealCreateDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public AppealTopic Topic { get; set; }
    public string Content { get; set; } = null!;
}