using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Dtos.ElectronicAppealDto;

public class ElectronicAppealFilterDto
{
    public bool? IsRead { get; set; }
    public bool? IsReplied { get; set; }
    public AppealTopic? Topic { get; set; }
}