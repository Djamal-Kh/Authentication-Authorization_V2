using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.Auth;
using Web.Domain;
using Web.Persistance;
using Web.Services;

namespace Web;

public static class ApiExtensions
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings!.SecretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["cookies"];

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<JwtProvider>();
        services.AddScoped<PasswordHasher>();
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorization();
    }
    
    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder, params EnumPermission[] permissions)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder
            .RequireAuthorization(pb =>
                pb.AddRequirements(new PermissionRequirement(permissions)));
    }
}