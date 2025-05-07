namespace AzerIsiq.Dtos.ElectronicAppealDto;

public class ElectronicAppealStatisticsDto
{
    public int TotalAppeals { get; set; }
    public int ReadAppeals { get; set; }
    public int UnreadAppeals { get; set; }
    public int RepliedAppeals { get; set; }
    public int NotRepliedAppeals { get; set; }
    public Dictionary<string, int> ByTopic { get; set; } = new();
    public Dictionary<string, int> MonthlyAppeals { get; set; } = new();
}