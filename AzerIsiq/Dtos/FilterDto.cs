namespace AzerIsiq.Dtos;

public class FilterDto
{
    public string SearchTerm { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? SubstationId { get; set; }
}