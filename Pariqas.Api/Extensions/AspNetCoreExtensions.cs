﻿using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Pariqas.Api.Data;
using Pariqas.Api.Services;
using Pariqas.Models.Identity;

namespace Pariqas.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<JwtService>();
        services.AddAuthentication(options => 
        { 
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            };
            
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Query[Constants.AccessToken];
                    var path = context.HttpContext.Request.Path;
                    
                    if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = token;
                    }
                    
                    return Task.CompletedTask;
                },
                
                OnAuthenticationFailed = context => Task.CompletedTask,
            };
        });
        
        return services;
    }
}