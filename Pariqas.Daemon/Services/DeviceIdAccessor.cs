using System.Net.Http.Json;

using Pariqas.Http.Requests;
using Pariqas.Models.Devices;

namespace Pariqas.Daemon.Services;

public sealed class DeviceIdAccessor
{
    private readonly Device _currentDevice;
    private readonly HttpClient _http;
    
    public DeviceIdAccessor(Device currentDevice, HttpClient http)
    {
        _currentDevice = currentDevice;
        _http = http;
    }

    public async Task<string> GetDeviceIdAsync()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "device-id.txt");
        if (File.Exists(path))
        {
            string content = await File.ReadAllTextAsync(path);
            if (!string.IsNullOrWhiteSpace(content) && Guid.TryParse(content, out var id))
            {
                return id.ToString();
            }
        }
        
        await CreateDeviceAsync();
        await File.WriteAllTextAsync(path, _currentDevice.Id);
        
        return _currentDevice.Id;
    }

    private async Task CreateDeviceAsync()
    {
        var response = await _http.PostAsJsonAsync("devices", new CreateDeviceRequest(Environment.MachineName, DeviceType.Computer));
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Failed to create device.");
        }
        
        var device = await response.Content.ReadFromJsonAsync<Device>();
        if (device is null)
        {
            
        }
        
        _currentDevice.Id = device.Id;
        _currentDevice.Name = device.Name;
        _currentDevice.Type = device.Type;
        _currentDevice.IsPrimary = device.IsPrimary;
        _currentDevice.UserId = device.UserId;
    }
}