namespace AzerIsiq.Models;

public class Counter
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public string StampCode { get; set; } = null!;
    public int Coefficient { get; set; }
    public string Volt { get; set; } = null!;
    public string Type { get; set; } = null!;
}