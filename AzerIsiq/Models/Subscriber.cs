using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Models;

public class Subscriber
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FinCode { get; set; } = null!;
    public PopulationStatus PopulationStatus { get; set; }
    public int RegionId { get; set; }
    public Region? Region { get; set; }
    public int DistrictId { get; set; }
    public District? District { get; set; }
    public int? TerritoryId { get; set; }
    public Territory? Territory { get; set; }
    public int? StreetId { get; set; }
    public Street? Street { get; set; }
    public string Building { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public string? Ats { get; set; }
    public string? SubscriberCode { get; set; }
    public int? CounterId { get; set; }
    public Counter? Counter { get; set; }
    public int? TmId { get; set; }
    public Tm? Tm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Status { get; set; } = 1;
}