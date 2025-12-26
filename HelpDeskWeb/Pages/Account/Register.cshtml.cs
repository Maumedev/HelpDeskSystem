using System.ComponentModel.DataAnnotations;
using HelpDesk.Core.Modules.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskWeb.Pages.Account;

public class InputModelRegistro
{

    public string? FullName { get; set; }

    [Required(ErrorMessage = "El usuario es necesario")]
    [MinLength(3, ErrorMessage = "La longitud del usuario no es válida")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "El email es necesario")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "El departamento es necesario")]
    public string Department { get; set; }

    [Required(ErrorMessage = "El nivel es necesario")]
    public string SeniorityLevel { get; set; }

    [Required(ErrorMessage = "El password es necesario")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

public class RegisterModel(UserManager<AppUser> userManager) : PageModel
{

    [BindProperty]
    public InputModelRegistro Input { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page(); // Si hay errores, volvemos a mostrar la página con los mensajes
        }
        var user = new AppUser
        {
            UserName = Input.Username,
            Email = Input.Email,
            FullName = Input.FullName,
            Departament = Input.Department,
            SeniorityLevel = Convert.ToInt16(Input.SeniorityLevel)
        };
        var result = await userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Customer");
            
            // ÉXITO: Redirigimos al Login para que inicie sesión
            return RedirectToPage("/Account/Login");
        }
        ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
        // Si falló (ej: password muy débil), agregamos los errores al HTML
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
