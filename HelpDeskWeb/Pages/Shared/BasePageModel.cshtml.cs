using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskWeb.Pages.Shared;
public class BasePageModelModel : PageModel
{
    // Usamos TempData para que el mensaje sobreviva a un RedirectToPage
    public void NotifySuccess(string message) => TempData["ToastMessage"] = $"success|{message}";
    public void NotifyError(string message) => TempData["ToastMessage"] = $"error|{message}";

    // Si quieres que el loader aparezca al CARGAR la página
    public void ForceLoaderOnLoad() => TempData["ShowLoader"] = "true";
}
