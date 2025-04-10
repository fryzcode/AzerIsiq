namespace AzerIsiq.Dtos;

public class DistrictDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }
    public RegionDto Region { get; set; }

}