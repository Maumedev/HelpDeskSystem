using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskWeb.Pages.Customer
{

    public class TicketViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Requester { get; set; } // Nombre del solicitante
        public string Category { get; set; }
        public string Priority { get; set; } // Low, Medium, High
        public string Status { get; set; }   // Open, Pending, Resolved
        public DateTime CreatedAt { get; set; }
    }
    public class MyTicketsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
