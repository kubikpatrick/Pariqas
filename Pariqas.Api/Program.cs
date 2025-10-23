using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

using Pariqas.Api.Data;
using Pariqas.Api.Extensions;

namespace Pariqas.Api;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
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

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}
