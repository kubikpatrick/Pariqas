using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Pariqas.Api.Data;
using Pariqas.Api.Services.Managers;

namespace Pariqas.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DevicesController : AuthorizeControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly DeviceManager _deviceManager;
    
    public DevicesController(ApplicationDbContext context, DeviceManager deviceManager)
    {
        _context = context;
        _deviceManager = deviceManager;
    }
    
    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var devices = await _context.Devices.Where(d => d.UserId == CurrentUserId) .ToListAsync();
        
        return Ok(devices);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        var device = await _deviceManager.FindByIdAsync(id);
        if (device is null || device.UserId != CurrentUserId)
        {
            return NotFound();
        }
        
        await _deviceManager.DeleteAsync(device);
        
        return Ok();
    }
}