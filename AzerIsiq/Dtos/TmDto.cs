namespace AzerIsiq.Dtos;

public class TmDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int RegionId { get; set; } 
    public int DistrictId { get; set; }
    public int SubstationId { get; set; }
}