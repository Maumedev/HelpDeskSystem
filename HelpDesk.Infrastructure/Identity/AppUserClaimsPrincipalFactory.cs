using System;
using System.Security.Claims;
using HelpDesk.Core.Modules.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HelpDesk.Infrastructure.Identity;

public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
{
    public AppUserClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        if (!string.IsNullOrWhiteSpace(user.Departament))
        {
            identity.AddClaim(new Claim("Department", user.Departament));
        }

        if (!string.IsNullOrWhiteSpace(user.FullName))
        {
            identity.AddClaim(new Claim("FullName", user.FullName));
        }

        // Puedes agregar más, ej: SeniorityLevel
        identity.AddClaim(new Claim("SeniorityLevel", user.SeniorityLevel.ToString()));

        return identity;
    }
}
