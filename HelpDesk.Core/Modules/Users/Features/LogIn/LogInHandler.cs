using HelpDesk.Core.Modules.Users.Entities;
using HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider;
using HelpDesk.Core.Modules.Users.Repositories;
using HelpDesk.Core.Modules.Users.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using ROP;

namespace HelpDesk.Core.Modules.Users.Features.LogIn;

public class LogInHandler(
    UserManager<AppUser> userManager,
    IBearerTokenProvider bearerTokenProvider,
    IRefreshTokenRepository refreshTokenRepository)
{
    public async Task<Result<LoginResponse>> HandleAsync(LoginDto input)
    {
        List<Error> errors = [];

        var user = await userManager.FindByEmailAsync(input.UserName);

        if (user is null || !await userManager.CheckPasswordAsync(user, input.Password))
        {
            errors.Add(Error.Create("Usuario no autorizado"));
            return Result.Failure<LoginResponse>([.. errors], System.Net.HttpStatusCode.Unauthorized);
        }

        var bearerToken = await bearerTokenProvider.GenerateToken(user);

        var tokenRefresh = bearerTokenProvider.GenerateRefreshToken();

        await refreshTokenRepository.RevokeActiveTokens(user.Id);

        var refreshToken = RefreshToken.Create(tokenRefresh, bearerToken.TokenId, user.Id);

        await refreshTokenRepository.Add(refreshToken);

        return new LoginResponse(bearerToken.BearerToken, tokenRefresh);

    }
}
