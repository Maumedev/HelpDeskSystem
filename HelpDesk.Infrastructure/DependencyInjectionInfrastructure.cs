using HelpDesk.Core.Modules.Users.Entities;
using HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider;
using HelpDesk.Core.Modules.Users.Repositories;
using HelpDesk.Infrastructure.Middlewares;
using HelpDesk.Infrastructure.Persistence;
using HelpDesk.Infrastructure.Persistence.Repositories;
using HelpDesk.Infrastructure.Services;
using HelpDesk.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HelpDesk.Infrastructure;

public static class DependencyInjectionInfrastructure
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddDatabaseConnection(configuration)
        .AddIdentity()
        .AddJwtSettings(configuration)
        .AddInfrastructureServices()
        .AddRepositories()
        .AddLogging()
        .AddMiddlewares();


    private static IServiceCollection AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connStr = configuration.GetConnectionString("Default");
        services.AddDbContext<HelpDeskContext>(options => options.UseSqlServer(connStr));
        return services;
    }
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<HelpDeskContext>()
        .AddDefaultTokenProviders();

        return services;
    }
    private static IServiceCollection AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        services.AddAuthentication()
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidAudience = jwtSettings["Audience"],
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        => services
        .AddScoped<IBearerTokenProvider, BearerTokenProvider>();
    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
        .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    => services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

    public static async Task<IServiceProvider> AddUserRoles(this IServiceProvider services)
    {

        

        using (var scope = services.CreateScope())
        {
            var servicesint = scope.ServiceProvider;
            try
            {
                await SeedRoles.SeedRolesAsync(services);
            }
            catch (Exception ex)
            {
                //var logger = servicesint.GetRequiredService<ILogger<Program>>();
                //logger.LogError(ex, "Ocurrió un error al crear los roles iniciales.");
            }
        }

        return services;
    }
}

