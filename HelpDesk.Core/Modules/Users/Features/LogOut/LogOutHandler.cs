using HelpDesk.Core.Modules.Users.Repositories;
using ROP;

namespace HelpDesk.Core.Modules.Users.Features.LogOut;

public class LogOutHandler(
    IRefreshTokenRepository refreshTokenRepository)
{
    public async Task<Result<Unit>> HandleAsync(string UserId)
    {
        await refreshTokenRepository.RevokeActiveTokens(UserId);
        return Result.Success();
    }
}
