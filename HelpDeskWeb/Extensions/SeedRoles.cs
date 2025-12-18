using Microsoft.AspNetCore.Identity;

namespace HelpDeskWeb.Extensions;

public static class SeedRoles
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "Agent", "Customer" };

        foreach (var roleName in roleNames)
        {
            // Verificamos si existe, si no, lo creamos.
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
