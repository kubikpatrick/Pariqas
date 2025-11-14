using Microsoft.AspNetCore.Mvc;

using Pariqas.Api.Services.Managers;
using Pariqas.Http.Requests;
using Pariqas.Models;
using Pariqas.Models.Devices;

namespace Pariqas.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DevicesController : AuthorizeControllerBase
{
    private readonly DeviceManager _deviceManager;
    
    public DevicesController(DeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Device>>> Index()
    {
        var devices = await _deviceManager.FindForUserAsync(CurrentUserId);

        return devices;
    }
    
    [HttpPost]
    public async Task<ActionResult<Device>> Create([FromBody] CreateDeviceRequest request)
    {
        var device = new Device
        {
            Name = request.Name,
            Type = request.Type,
            UserId = CurrentUserId
        };
        
        var result = await _deviceManager.CreateAsync(device);
        if (!result.Succeeded)
        {
            return Problem(result.Message);
        }

        return device;
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        var device = await _deviceManager.FindByIdAsync(id, CurrentUserId);
        if (device is null)
        {
            return NotFound();
        }
        
        var result = await _deviceManager.DeleteAsync(device);
        if (!result.Succeeded)
        {
            return Problem(result.Message);
        }
        
        return Ok();
    }
    
    [HttpPatch("{id:guid}/location")]
    public async Task<ActionResult> Location([FromRoute] string id, Location location)
    {
        var device = await _deviceManager.FindByIdAsync(id, CurrentUserId);
        if (device is null)
        {
            return NotFound();
        }
        
        var result = await _deviceManager.UpdateLocationAsync(device, location);
        if (!result.Succeeded)
        {
            return Problem(result.Message);
        }
        
        return Ok();
    }
}