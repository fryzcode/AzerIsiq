using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Dtos;

public class SubscriberRequestDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FinCode { get; set; } = null!;
    public PopulationStatus PopulationStatus { get; set; }
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public string Building { get; set; } = null!;
    public string Apartment { get; set; } = null!;
}