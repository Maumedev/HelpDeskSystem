using HelpDesk.Core.Modules.Users.Features.LogIn;
using HelpDesk.Core.Modules.Users.Features.LogOut;
using HelpDesk.Core.Modules.Users.Features.RefreshTokens;
using HelpDesk.Core.Modules.Users.Features.RegisterIn;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;

namespace HelpDeskWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticatorController(
    LogInHandler logInHandler,
    RegisterInHandler registerInHandler,
    RefreshTokenHandler refreshTokenHandler,
    LogOutHandler logOutHandler
    ) : ControllerBase
{
    [HttpPost("log-in")]
    public async Task<IActionResult> UserLogin([FromBody] LoginDto input)
    {
        return await logInHandler.HandleAsync(input).ToActionResult();
    }

    [HttpPut("log-out")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public async Task<IActionResult> LogOut([FromQuery] string UserId)
    {
        return await logOutHandler.HandleAsync(UserId).ToActionResult();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto input)
    {
        return await refreshTokenHandler.HandleAsync(input).ToActionResult();
    }


    [HttpPost("register-in")]
    public async Task<IActionResult> UserRegister([FromBody] RegisterDto input)
    {
        return await registerInHandler.HandleAsync(input).ToActionResult();
    }
}
