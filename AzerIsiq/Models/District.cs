namespace AzerIsiq.Models;

public class District
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int RegionId { get; set; }
    public Region Region { get; set; } = null!;
    public int? CityId { get; set; }
    public City? City { get; set; }
    public ICollection<Substation> Substations { get; set; } = new List<Substation>();
}
