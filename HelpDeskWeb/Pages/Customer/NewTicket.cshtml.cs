using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelpDeskWeb.Pages.Customer;

public enum TicketPriority
{
    Low,    // Coincide con value="Low"
    Medium, // Coincide con value="Medium"
    High    // Coincide con value="High"
}

public class InputModel
        {
            [Required(ErrorMessage = "El asunto es obligatorio.")]
            [StringLength(100, MinimumLength = 5, ErrorMessage = "El asunto debe tener entre 5 y 100 caracteres.")]
            [Display(Name = "Asunto del Problema")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Debes seleccionar una categoría.")]
            [Display(Name = "Departamento")]
            public int? CategoryId { get; set; } // Nullable para validar que seleccionó algo

            [Required(ErrorMessage = "La prioridad es obligatoria.")]
            public TicketPriority Priority { get; set; }

            [Required(ErrorMessage = "Por favor describe el problema detalladamente.")]
            [StringLength(2000, ErrorMessage = "La descripción es demasiado larga.")]
            [Display(Name = "Descripción")]
            public string Description { get; set; }

            // IFormFile maneja la subida de archivos
            [Display(Name = "Archivo Adjunto")]
            public IFormFile? Upload { get; set; } 
        }

public class NewTicketModel : PageModel
{

    [BindProperty]
        public InputModel Input { get; set; }

        // Lista para el Dropdown de categorías
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList Categories { get; set; }

    public void OnGet()
    {
    }
}
