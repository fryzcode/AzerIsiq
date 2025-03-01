using System.ComponentModel.DataAnnotations.Schema;
using AzerIsiq.Models;

namespace AzerIsiq.Dtos;

public class SubstationDto
{
    [NotMapped]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int RegionId { get; set; } 
    public int DistrictId { get; set; }
}
