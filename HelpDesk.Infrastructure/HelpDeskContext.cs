
using HelpDesk.Core.Modules.Tickets.Data;
using HelpDesk.Core.Modules.Users.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure;

public class HelpDeskContext : IdentityDbContext<AppUser>
{
    public HelpDeskContext(DbContextOptions<HelpDeskContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Ticket>(entity =>
        {
            // Relación con Cliente
            entity.HasOne(t => t.Customer)
                  .WithMany(u => u.TicketsCreated)
                  .HasForeignKey(t => t.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict); // Si borras al usuario, no borres sus tickets históricos

            // Relación con Agente
            entity.HasOne(t => t.Agent)
                  .WithMany(u => u.TicketsAssigned)
                  .HasForeignKey(t => t.AgentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

    }
}
