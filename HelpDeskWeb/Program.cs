using HelpDesk.Core.Modules.Users.Data;
using HelpDesk.Infrastructure;
using HelpDeskWeb.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWeb;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services.AddControllers();


        // EF Core
        builder.Services.AddDbContext<HelpDeskContext>(options =>
            options.UseSqlServer("Data Source=localhost;Database=HelpDesk;User ID=sa;Password=123adminM$;Trust Server Certificate=True"));

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<HelpDeskContext>()
        .AddDefaultTokenProviders();


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // Llamamos a tu clase estática para crear roles si no existen
                await SeedRoles.SeedRolesAsync(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocurrió un error al crear los roles iniciales.");
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Esto evita que el navegador guarde el CSS en caché mientras programas
                    ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
                }
            });
        }
        else
        {
            app.UseStaticFiles();
        }


        app.MapControllers();

        app.UseHttpsRedirection();
        //app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
