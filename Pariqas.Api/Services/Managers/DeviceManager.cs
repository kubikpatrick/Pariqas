using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Pariqas.Api.Data;
using Pariqas.Api.Hubs;
using Pariqas.Models;
using Pariqas.Models.Devices;

namespace Pariqas.Api.Services.Managers;

public sealed class DeviceManager
{
    private readonly ApplicationDbContext _context;

    private readonly IHubContext<LocationHub> _hub;
    private readonly IMemoryCache _cache;
    
    public DeviceManager(ApplicationDbContext context, IHubContext<LocationHub> hub, IMemoryCache cache)
    {
        _context = context;
        _hub = hub;
        _cache = cache;
    }

    public async Task<Device?> FindByIdAsync(string deviceId, string userId)
    {
        if (_cache.TryGetValue(deviceId, out Device? device))
        {
            return device.UserId == userId ? device : null;
        }

        device = await _context.Devices.FindAsync(deviceId);
        if (device is not null && device.UserId == userId)
        {
            _cache.Set(deviceId, device, TimeSpan.FromMinutes(15));
            
            return device;
        }

        return null;
    }

    public async Task<List<Device>> FindForUserAsync(string userId)
    {
        return await _context.Devices.Where(d => d.UserId == userId).ToListAsync();
    }
    
    public async Task<OperationResult> CreateAsync(Device device)
    {
        if (string.IsNullOrEmpty(device.Name))
        {
            return OperationResult.Fail("Device name cannot be empty.");
        }
        
        await _context.Devices.AddAsync(device);
        await _context.SaveChangesAsync();

        return OperationResult.Success();
    }
    
    public async Task<OperationResult> DeleteAsync(Device device)
    {
        if (_cache.TryGetValue(device.Id, out _))
        {
            _cache.Remove(device.Id);
        }
        
        _context.Devices.Remove(device);
        
        await _context.SaveChangesAsync();
        
        return OperationResult.Success();
    }
    
    public async Task<OperationResult> UpdateLocationAsync(Device device, Location location)
    {
        var now = DateTime.UtcNow;
        if (location.Timestamp < now.AddMinutes(-1) || location.Timestamp > now.AddMinutes(1))
        {
            return OperationResult.Fail("Location timestamp is incoherent.");
        }
        
        if (device.Location.Longitude == location.Longitude && device.Location.Latitude == location.Latitude)
        {
            return OperationResult.Success();
        }
        
        device.Location = location;
        _context.Devices.Update(device);
        _cache.Set(device.Id, device, TimeSpan.FromMinutes(15));
        
        await _context.SaveChangesAsync();
        await _hub.Clients.User(device.UserId).SendAsync(EventNames.LocationUpdate, device.Id, location);
        
        return OperationResult.Success();
    }
}