using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Pariqas.Models.Identity;

namespace Pariqas.Models.Devices;

[PrimaryKey(nameof(Id))]
public sealed class Device
{
    public Device()
    {
    }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public bool IsPrimary { get; set; }
    
    [Required]
    public DeviceType Type { get; set; }

    [Required]
    public Location Location { get; set; } = Location.Empty;
    
    [Required]
    public string UserId { get; set; }
    
    [NotMapped]
    public User User { get; set; }
}