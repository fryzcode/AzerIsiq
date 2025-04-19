namespace AzerIsiq.Models;

public class Location
{
    public int Id { get; set; }
    public decimal Latitude { get; set; } 
    public decimal Longitude { get; set; } 
    public string Address { get; set; } = string.Empty; 

    public ICollection<Substation> Substations { get; set; } = new List<Substation>();
    public ICollection<Tm> Tms { get; set; } = new List<Tm>();
}