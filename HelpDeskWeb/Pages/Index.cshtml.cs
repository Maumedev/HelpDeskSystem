using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HelpDeskWeb.Pages.Shared;

namespace HelpDeskWeb.Pages
{
    public class IndexModel : BasePageModelModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // NotifySuccess("¡Aplicación cargada con éxito!");
            // NotifyError("¡Aplicación con error!");
        }
    }
}
