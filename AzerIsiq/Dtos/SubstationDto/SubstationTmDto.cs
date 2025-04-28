namespace AzerIsiq.Dtos;

public class SubstationTmDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int DistrictId { get; set; }
    public DistrictDto District { get; set; }
}