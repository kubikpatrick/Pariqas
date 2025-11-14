using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Pariqas.Models.Identity;

[PrimaryKey(nameof(Id))]
public sealed class RefreshToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Token { get; set; }
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    [Required]
    public bool IsRevoked { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}