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
    public int CityId { get; set; }
    public City? City { get; set; }
    public string District { get; set; } = null!;
    public string Building { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public string? AtsCode { get; set; }
    public int CounterId { get; set; }
    public Counter? Counter { get; set; }
    public int TmId { get; set; }
    public Tm? Tm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Status { get; set; } = 1;
}