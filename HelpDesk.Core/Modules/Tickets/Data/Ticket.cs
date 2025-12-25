using HelpDesk.Core.Modules.Users.Entities;

namespace HelpDesk.Core.Modules.Tickets.Data;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // RELACIÓN 1: El Cliente (Quien crea el ticket)
    // El ID de Identity es un string (GUID) por defecto.
    public string CustomerId { get; set; }
    public AppUser Customer { get; set; }

    // RELACIÓN 2: El Agente (Quien atiende el ticket)
    // Puede ser null si nadie lo ha tomado aún.
    public string? AgentId { get; set; }
    public AppUser? Agent { get; set; }
}
