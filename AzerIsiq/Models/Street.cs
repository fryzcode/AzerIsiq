namespace AzerIsiq.Models;

public class Street
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int TerritoryId { get; set; }
    public Territory Territory { get; set; } = null!;
}