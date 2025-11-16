using Pariqas.Web.Handlers;
using Pariqas.Web.Services;

namespace Pariqas.Web.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        return services.AddScoped(sp =>
        {
            var tokenService = sp.GetRequiredService<TokenService>();
            var serverUrlService = sp.GetRequiredService<ServerUrlService>();

            return new HttpClient(new JwtAuthorizationHandler(tokenService, serverUrlService));
        });
    }
}