using Community.Blazor.MapLibre;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Marker;

using Pariqas.Models;

namespace Pariqas.Web.Services;

public sealed class MarkerService
{
    private readonly MapLibre _map;
    
    public MarkerService(MapLibre map)
    {
        _map = map;
    }
    
    public async Task AddMarkerAsync(string id, Location location, MarkerOptions options)
    {
        await _map.AddMarker(options, new LngLat(location.Longitude, location.Latitude), new Guid(id));
    }
    
    public async Task RemoveMarkerAsync(string id)
    {
        await _map.RemoveMarker(new Guid(id));
    }

    public async Task MoveMarkerAsync(string id, Location location)
    {
        await _map.MoveMarker(new Guid(id), new LngLat(location.Longitude, location.Latitude));
    }
}