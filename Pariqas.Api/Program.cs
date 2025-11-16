using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using Pariqas.Api.Data;
using Pariqas.Api.Extensions;

namespace Pariqas.Api;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddLogging();
        builder.Services.AddSignalR();
        builder.Services.AddIdentity();
        builder.Services.AddManagers();
        builder.Services.AddTokenAuthentication(builder.Configuration);
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
        });
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            bool created = await db.Database.EnsureCreatedAsync();
            if (!created)
            {
                await db.Database.MigrateAsync();
            }
        }

        app.UseHttpsRedirection();
        app.UseCors(cors => cors.AllowAnyHeader().AllowAnyOrigin().AllowAnyOrigin());
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHubs();

        await app.RunAsync();
    }
}
