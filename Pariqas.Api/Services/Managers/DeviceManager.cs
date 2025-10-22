using Microsoft.Extensions.Caching.Memory;

using Pariqas.Api.Data;
using Pariqas.Models.Devices;

namespace Pariqas.Api.Services.Managers;

public sealed class DeviceManager
{
    private readonly ApplicationDbContext _context;

    private readonly IMemoryCache _cache;
    
    public DeviceManager(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Device?> FindByIdAsync(string deviceId)
    {
        if (_cache.TryGetValue(deviceId, out Device? device))
        {
            return device;
        }

        device = await _context.Devices.FindAsync(deviceId);
        if (device is not null)
        {
            _cache.Set(deviceId, device, TimeSpan.FromMinutes(15));
        }

        return device;
    }
    

    public async Task DeleteAsync(Device device)
    {
        if (_cache.TryGetValue(device.Id, out _))
        {
            _cache.Remove(device.Id);
        }

        _context.Devices.Remove(device);

        await _context.SaveChangesAsync();
    }
}