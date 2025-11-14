namespace Pariqas.Daemon.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            
            return new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("Credentials:ServerUrl") ?? throw new InvalidOperationException())
            };
        });
    }
}