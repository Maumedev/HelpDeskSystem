using HelpDesk.Core.Modules.Users.Features.LogIn;
using HelpDesk.Core.Modules.Users.Features.LogOut;
using HelpDesk.Core.Modules.Users.Features.RefreshTokens;
using HelpDesk.Core.Modules.Users.Features.RegisterIn;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Core.Modules.Users;

public static class DependencyInjectionUserModule
{
    public static IServiceCollection UsersModuleAddFeatures(this IServiceCollection services)
        => services
        .AddScoped<LogInHandler>()
        .AddScoped<RegisterInHandler>()
        .AddScoped<RefreshTokenHandler>()
        .AddScoped<LogOutHandler>();
}
