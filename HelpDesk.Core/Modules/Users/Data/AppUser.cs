using HelpDesk.Core.Modules.Tickets.Data;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Core.Modules.Users.Data;

public class AppUser : IdentityUser
{
    public string Departament { get; set; }
    public string FullName { get; set; }
    public int SeniorityLevel { get; set; }

    public ICollection<Ticket> TicketsCreated { get; set; }
    public ICollection<Ticket> TicketsAssigned { get; set; }
}
