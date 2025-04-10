namespace AzerIsiq.Dtos;

public class TmDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int RegionId { get; set; } 
    public int DistrictId { get; set; }
    public int SubstationId { get; set; }
    public string? Latitude { get; set; } 
    public string? Longitude { get; set; } 
    public string? Address { get; set; }
}