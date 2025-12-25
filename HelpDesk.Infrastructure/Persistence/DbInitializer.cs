

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Infrastructure.Persistence;

public static class DbInitializer
{
    public async static void InitDb(WebApplication app)
    {
        string[] roleNames = ["Admin", "Agent", "Customer"];
        
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<HelpDeskContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            context.Database.Migrate();
            Console.WriteLine("--> Base de Datos lista.");

            foreach (var roleName in roleNames)
            {
                // Verificamos si existe, si no, lo creamos.
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            Console.WriteLine("--> Datos de Seed insertados.");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> Error aplicando migraciones: {ex.Message}");
        }
    }

}
