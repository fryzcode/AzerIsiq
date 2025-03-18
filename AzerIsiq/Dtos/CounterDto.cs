namespace AzerIsiq.Dtos;

public class CounterDto
{
    public string Number { get; set; } = null!;
    public string StampCode { get; set; } = null!;
    public int Coefficient { get; set; }
    public string Volt { get; set; } = null!;
    public string Type { get; set; } = null!;
}