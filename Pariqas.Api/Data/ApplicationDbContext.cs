using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Pariqas.Models.Devices;
using Pariqas.Models.Identity;

namespace Pariqas.Api.Data;

public sealed class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Device> Devices { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}