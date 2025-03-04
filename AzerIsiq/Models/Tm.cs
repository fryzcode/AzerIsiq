namespace AzerIsiq.Models;

public class Tm
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SubstationId { get; set; }
    public Substation Substation { get; set; } = null!;
    public ICollection<ImageEntity> Images { get; set; } = new List<ImageEntity>();
}