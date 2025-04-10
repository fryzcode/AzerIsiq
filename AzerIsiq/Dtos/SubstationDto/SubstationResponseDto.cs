namespace AzerIsiq.Dtos;

public class SubstationResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int RegionId { get; set; } 
    public int DistrictId { get; set; }
    // public decimal Latitude { get; set; }
    // public decimal Longitude { get; set; }
    // public string? Address { get; set; }
    // public IFormFile Image { get; set; } 
}