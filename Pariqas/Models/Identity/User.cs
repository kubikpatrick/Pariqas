using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

using Pariqas.Models.Devices;

namespace Pariqas.Models.Identity;

public sealed class User : IdentityUser
{
    public User()
    {
        
    }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [NotMapped]
    public List<Device> Devices { get; set; }
}