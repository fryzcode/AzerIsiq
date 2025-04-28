namespace AzerIsiq.Dtos;

public class SubscriberProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FinCode { get; set; } = null!;
    public string PopulationStatus { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string District { get; set; } = null!;
    public string? Territory { get; set; }
    public string? Street { get; set; }
    public string Address { get; set; } = null!;
    public string? Ats { get; set; }
    public string? SubscriberCode { get; set; }
    public string RequestStatus { get; set; } = null!;
}