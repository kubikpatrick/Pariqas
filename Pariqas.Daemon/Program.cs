using Pariqas.Daemon.Extensions;
using Pariqas.Daemon.Services;
using Pariqas.Daemon.Workers;
using Pariqas.Models.Devices;

namespace Pariqas.Daemon;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddLogging();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton(new Device());
        builder.Services.AddSingleton<DeviceIdAccessor>();
        builder.Services.AddSingleton<JwtService>();
        builder.Services.AddHostedService<JwtRefreshWorker>();
        builder.Services.AddHostedService<LocationWorker>();

        var host = builder.Build();
        
        host.Run();
    }
}