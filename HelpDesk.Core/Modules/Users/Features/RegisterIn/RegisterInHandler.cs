using HelpDesk.Core.Modules.Users.Entities;
using Microsoft.AspNetCore.Identity;
using ROP;

namespace HelpDesk.Core.Modules.Users.Features.RegisterIn;

public class RegisterInHandler(
    UserManager<AppUser> userManager)
{
    public async Task<Result<Unit>> HandleAsync(RegisterDto input)
    {
        var newUser = new AppUser
        {
            UserName = input.Username,
            Departament = input.Department,
            Email = input.Email,
            FullName = input.FullName,
            SeniorityLevel = input.SeniorityLevel
        };

        var result = await userManager.CreateAsync(newUser, input.Password);

        if (!result.Succeeded)
        {
            List<Error> errors = [.. result.Errors.Select(e =>
            {
                return Error.Create(e.Description);
            })];

            return Result.Failure<Unit>([..errors], System.Net.HttpStatusCode.Conflict);
        }

        await userManager.AddToRoleAsync(newUser, "Customer");

        return Result.Success();
    }
}
