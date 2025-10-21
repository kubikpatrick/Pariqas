using Microsoft.AspNetCore.Mvc;

using Pariqas.Api.Services;

namespace Pariqas.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly JwtService _jwt;
    
    public AuthenticationController(JwtService jwt)
    {
        _jwt = jwt;
    }
}