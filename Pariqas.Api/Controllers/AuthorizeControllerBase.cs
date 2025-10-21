using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Pariqas.Api.Controllers;

[Authorize]
public abstract class AuthorizeControllerBase : ControllerBase
{
    protected AuthorizeControllerBase()
    {
        CurrentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
    }

    protected readonly string CurrentUserId;
}