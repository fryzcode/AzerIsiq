namespace AzerIsiq.Dtos;

public class SubscriberDebtDto
{
    public string SubscriberCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string DistrictName { get; set; } = null!;
    public int TotalCurrentValue { get; set; }
    public decimal Debt { get; set; }
}
