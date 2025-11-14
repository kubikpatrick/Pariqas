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
        Longitude = default,
        Latitude = default,
        Timestamp = default
    };
    
    public static int CalculateDistance(Location location1, Location location2)
    {
        const double R = 6371.0;

        double lat1 = location1.Latitude * Math.PI / 180.0;
        double lon1 = location1.Longitude * Math.PI / 180.0;
        double lat2 = location2.Latitude * Math.PI / 180.0;
        double lon2 = location2.Longitude * Math.PI / 180.0;
        
        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;
        
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = R * c;

        return (int)Math.Round(distance);
    }

}