using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pariqas.Api.Controllers;

[Authorize]
public abstract class AuthorizeControllerBase : ControllerBase
{
    protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}