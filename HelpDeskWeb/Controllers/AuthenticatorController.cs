using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticatorController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Example()
    {
        return Ok("Running controller");
    }

}
