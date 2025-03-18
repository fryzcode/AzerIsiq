namespace AzerIsiq.Models;

public class Substation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DistrictId { get; set; }
    public District District { get; set; } = null!;
    public ICollection<Tm> Tms { get; set; } = new List<Tm>();
    public int? LocationId { get; set; }
    public Location? Location { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
}