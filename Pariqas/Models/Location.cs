using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Pariqas.Models;

[Owned]
public sealed class Location
{
    public Location()
    {
    }

    [Required]
    public double Longitude { get; set; }
    
    [Required]
    public double Latitude { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    [NotMapped]
    public int ReferenceId { get; set; }

    [NotMapped]
    public static Location Empty => new Location
    {
        Longitude = 0,
        Latitude = 0,
        Timestamp = DateTime.UtcNow
    };
}