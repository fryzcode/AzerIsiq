namespace AzerIsiq.Models;

public class Tm
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SubstationId { get; set; }
    public Substation Substation { get; set; } = null!;
    public int? LocationId { get; set; }
    public Location? Location { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
}