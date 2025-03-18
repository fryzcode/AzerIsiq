namespace AzerIsiq.Models;

public class District
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int RegionId { get; set; }
    public Region Region { get; set; } = null!;
    public ICollection<Substation> Substations { get; set; } = new List<Substation>();
    public ICollection<Territory> Territories { get; set; } = new List<Territory>();
}
