using Pariqas.Daemon.Services;

namespace Pariqas.Daemon.Workers;

public sealed class JwtRefreshWorker : BackgroundService
{
    private readonly JwtService _jwtService;

    public JwtRefreshWorker(JwtService jwtService)
    {
        _jwtService = jwtService;
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _jwtService.InitializeAsync();
        await base.StartAsync(cancellationToken);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _jwtService.RefreshAccessTokenAsync();
            await Task.Delay(TimeSpan.FromMinutes(14), stoppingToken);
        }
    }
}