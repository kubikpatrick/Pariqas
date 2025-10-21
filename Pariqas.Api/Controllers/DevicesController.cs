using Microsoft.AspNetCore.Mvc;

namespace Pariqas.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DevicesController : AuthorizeControllerBase
{
    public DevicesController()
    {
        
    }
}