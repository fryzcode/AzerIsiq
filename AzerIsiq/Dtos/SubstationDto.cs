using AzerIsiq.Models;

namespace AzerIsiq.Dtos;

public class SubstationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }
    public int DistrictId { get; set; }
}