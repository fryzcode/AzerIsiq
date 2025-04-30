namespace AzerIsiq.Models;

public class Counter
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public string StampCode { get; set; } = null!;
    public int Coefficient { get; set; }
    public string Volt { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Phase { get; set; }
    public int CurrentValue { get; set; }
    public int Status { get; set; } = 1;
    // public int SubscriberId { get; set; }
    // public Subscriber Subscriber { get; set; } = null!;
    public int? SubscriberId { get; set; }
    public Subscriber? Subscriber { get; set; }

}