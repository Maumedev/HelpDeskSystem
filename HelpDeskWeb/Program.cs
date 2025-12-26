using HelpDesk.Core.Modules.Users;
using HelpDesk.Infrastructure;

namespace HelpDeskWeb;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages(options =>
        {
            // ESTRATEGIA: Bloquear todo por defecto (Lista blanca)
            // Esto asegura que ninguna página sea accesible a menos que digamos lo contrario.
            options.Conventions.AuthorizeFolder("/"); 
            // Permitir acceso anónimo a la página de Login (la crearemos más adelante)
            options.Conventions.AllowAnonymousToPage("/Account/Login");
            options.Conventions.AllowAnonymousToPage("/Account/Register");
        });

        builder.Services.AddControllers();

        builder.Services.AddInfrastructureLayer(builder.Configuration);

        builder.Services.UsersModuleAddFeatures();

        var app = builder.Build();

        app.UseExceptionHandler();

        await app.PrepareDbData();


        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
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

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
