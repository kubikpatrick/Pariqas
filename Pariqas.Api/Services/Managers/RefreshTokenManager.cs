using Microsoft.EntityFrameworkCore;

using Pariqas.Api.Data;
using Pariqas.Models.Identity;

namespace Pariqas.Api.Services.Managers;

public sealed class RefreshTokenManager
{
    private readonly ApplicationDbContext _context;
    
    public RefreshTokenManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> FindByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rf => rf.User)
            .SingleOrDefaultAsync(rf => rf.Token == token && !rf.IsRevoked);
    }

    public async Task<RefreshToken?> CreateAsync(User user)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid().ToString(),
            Token = Guid.NewGuid().ToString().Replace("-", ""),
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false,
            UserId = user.Id
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task RevokeAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FindAsync(token);
        if (refreshToken is not null)
        {
            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public bool IsValid(RefreshToken refreshToken)
    {
        return refreshToken.ExpiresAt >= DateTime.UtcNow && !refreshToken.IsRevoked;
    }
}