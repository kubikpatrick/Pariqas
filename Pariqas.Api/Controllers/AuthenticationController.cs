using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Pariqas.Api.Services;
using Pariqas.Api.Services.Managers;
using Pariqas.Http.Requests;
using Pariqas.Http.Responses;
using Pariqas.Models.Identity;

namespace Pariqas.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly JwtService _jwt;
    private readonly UserManager<User> _userManager;
    private readonly RefreshTokenManager _refreshTokenManager;
    
    public AuthenticationController(JwtService jwt, UserManager<User> userManager, RefreshTokenManager refreshTokenManager)
    {
        _jwt = jwt;
        _userManager = userManager;
        _refreshTokenManager = refreshTokenManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NotFound("User does not exist.");
        }
        
        bool valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            return Unauthorized();       
        }
        
        string token = _jwt.GenerateToken(user);
        var refreshToken = await _refreshTokenManager.CreateAsync(user);
        if (refreshToken is null)
        {
            return StatusCode(500);       
        }

        return new TokenResponse(token, refreshToken.Token);
    }
    
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp([FromBody] SignUpRequest request)
    {
        bool exists = await _userManager.FindByEmailAsync(request.Email) is not null;
        if (exists)
        {
            return Conflict();
        }

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> Refresh([FromBody] RefreshRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return Unauthorized();
        }
        
        var refreshToken = await _refreshTokenManager.FindByTokenAsync(request.RefreshToken);
        if (refreshToken is null || !_refreshTokenManager.IsValid(refreshToken))
        {
            return Unauthorized();
        }

        string newToken = _jwt.GenerateToken(refreshToken.User);

        return new TokenResponse(newToken);
    }
}