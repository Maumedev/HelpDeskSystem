using System.ComponentModel.DataAnnotations;
using HelpDesk.Core.Modules.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace HelpDeskWeb.Pages.Account;

public class InputModel
{
    [Required(ErrorMessage = "El usuario es necesario")]
    [MinLength(3, ErrorMessage = "La longitud del usuario no es válida")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "El password es necesario")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

public class LoginModel(SignInManager<AppUser> signInManager) : PageModel
{

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }


    public IActionResult OnGet(string? returnUrl = null)
    {
        // 1. Si ya está autenticado, redirigir al Index automáticamente
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToPage("/Index");
        }

        ReturnUrl = returnUrl ?? Url.Content("~/");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid) return Page();

        var result = await signInManager.PasswordSignInAsync(Input.Username, Input.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            return Page();
        }

        return LocalRedirect(returnUrl);
    }
}
