using HelpDesk.Core.Modules.Users;
using HelpDesk.Infrastructure;

namespace HelpDeskWeb;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services.AddControllers();

        builder.Services.AddInfrastructureLayer(builder.Configuration);

        builder.Services.UsersModuleAddFeatures();

        var app = builder.Build();

        app.UseExceptionHandler();

        await app.Services.AddUserRoles();


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

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
