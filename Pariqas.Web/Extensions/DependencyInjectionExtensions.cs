using Pariqas.Web.Handlers;
using Pariqas.Web.Services;

namespace Pariqas.Web.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        return services.AddScoped(sp => new HttpClient(new JwtAuthorizationHandler(sp.GetRequiredService<TokenService>())));
    }
}