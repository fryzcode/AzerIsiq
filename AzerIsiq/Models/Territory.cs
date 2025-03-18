namespace AzerIsiq.Models;

public class Territory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DistrictId { get; set; }
    public District District { get; set; } = null!;
    public ICollection<Street> Streets { get; set; } = new List<Street>();
}