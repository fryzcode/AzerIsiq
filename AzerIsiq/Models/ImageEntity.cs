namespace AzerIsiq.Models;

public class ImageEntity
{
    public int Id { get; set; }
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
    public string? ImageName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? SubstationId { get; set; }
    public Substation? Substation { get; set; }

    public int? TmId { get; set; }
    public Tm? Tm { get; set; }
}

