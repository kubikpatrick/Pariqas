using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Pariqas.Web.Extensions;
using Pariqas.Web.Services;

namespace Pariqas.Web;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddAuthorizationCore();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<ServerUrlService>();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

        await builder.Build().RunAsync();
    }
}