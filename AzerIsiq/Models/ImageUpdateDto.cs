namespace AzerIsiq.Models;

public class ImageUpdateDto
{
    public int Id { get; set; }
    public IFormFile File { get; set; }
    public int? SubstationId { get; set; }
    public int? TmId { get; set; }
}