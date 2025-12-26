using HelpDesk.Core.Modules.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskWeb.Pages.Account;
public class LogoutModel(SignInManager<AppUser> signInManager) : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await signInManager.SignOutAsync(); 
        return RedirectToPage("/Account/Login");
    }
}