using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Pariqas.Api.Hubs;

[Authorize]
public sealed class LocationHub : Hub
{
    public LocationHub()
    {
        
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}