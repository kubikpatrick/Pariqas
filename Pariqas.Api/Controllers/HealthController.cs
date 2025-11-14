using Microsoft.AspNetCore.Mvc;

namespace Pariqas.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class HealthController : ControllerBase
{
    public HealthController()
    {
        
    }
    
    [HttpGet]
    public ActionResult Index() => Ok();
}