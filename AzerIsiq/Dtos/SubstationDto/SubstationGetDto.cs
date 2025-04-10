namespace AzerIsiq.Dtos;

public class SubstationGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DistrictDto District { get; set; }
    public LocationDto? Location { get; set; }
    public List<ImageDto> Images { get; set; }
}

public class ImageDto
{
    public int Id { get; set; }
    public string ImageName { get; set; }
}
