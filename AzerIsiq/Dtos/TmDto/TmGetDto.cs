namespace AzerIsiq.Dtos;

public class TmGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public SubstationTmDto Substation { get; set; }
    public LocationDto? Location { get; set; }
    public List<ImageDto> Images { get; set; }
}
