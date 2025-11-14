using System.Net.Http.Json;

using Pariqas.Daemon.Services;
using Pariqas.Models;

using Windows.Devices.Geolocation;

namespace Pariqas.Daemon.Workers;

public sealed class LocationWorker : BackgroundService
{
    private readonly DeviceIdAccessor _deviceIdAccessor;
    private readonly HttpClient _http;
    
    private readonly ILogger<LocationWorker> _logger;
    
    public LocationWorker(DeviceIdAccessor deviceIdAccessor, HttpClient http, ILogger<LocationWorker> logger)
    {
        _deviceIdAccessor = deviceIdAccessor;
        _http = http;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var access = await Geolocator.RequestAccessAsync();
        if (access is not GeolocationAccessStatus.Allowed)
        {
            throw new Exception("Location access denied.");
        }
        
        var geolocator = new Geolocator
        {
            DesiredAccuracy = PositionAccuracy.High,
            MovementThreshold = 3
        };
        
        geolocator.PositionChanged += OnLocationChange;
        
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async void OnLocationChange(object sender, PositionChangedEventArgs e)
    {
        double longitude = e.Position.Coordinate.Longitude;
        double latitude = e.Position.Coordinate.Latitude;

        var location = new Location
        {
            Longitude = longitude,
            Latitude = latitude,
            Timestamp = DateTime.UtcNow
        };
        
        try
        {
            string id = await _deviceIdAccessor.GetDeviceIdAsync();
            
            var response = await _http.PatchAsJsonAsync($"devices/{id}/location", location);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("[{timestamp}] - Code: {Code}, reason: {Reason}", DateTime.UtcNow, response.StatusCode, response.ReasonPhrase);
            }
            else
            {
                _logger.LogInformation("[{timestamp}] - Location updated successfully.", DateTime.UtcNow);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("[{timestamp}] - {message}" , DateTime.UtcNow, ex.Message);
            
            throw;
        }
    }
}