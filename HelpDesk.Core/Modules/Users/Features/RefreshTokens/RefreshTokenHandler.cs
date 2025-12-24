using HelpDesk.Core.Modules.Users.Entities;
using HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider;
using HelpDesk.Core.Modules.Users.Repositories;
using HelpDesk.Core.Modules.Users.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using ROP;

namespace HelpDesk.Core.Modules.Users.Features.RefreshTokens;

public class RefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IBearerTokenProvider tokenProvider,
    UserManager<AppUser> userManager)
{
    public async Task<Result<LoginResponse>> HandleAsync(RefreshTokenDto input)
    {

        var refreshToken = await refreshTokenRepository.GetByTokenId(input.RefreshToken);

        if (refreshToken is null) return Result.Failure<LoginResponse>("Token inválido");

        var markUsedResult = refreshToken.MarkAsUsed();

        if (!markUsedResult.Success) return Result.Failure<LoginResponse>(markUsedResult.Errors);

        var user = await userManager.FindByIdAsync(refreshToken.UserId);

        if (user is null)
            throw new InvalidOperationException("Inconsistencia de datos: Token existe sin usuario.");

        var accessToken = await tokenProvider.GenerateToken(user);

        var newRefreshValue = tokenProvider.GenerateRefreshToken();

        var newRefreshToken = RefreshToken.Create(
            newRefreshValue,
            accessToken.TokenId,
            user.Id
        );

        await refreshTokenRepository.Update(refreshToken);
        await refreshTokenRepository.Add(newRefreshToken);

        return new LoginResponse(accessToken.BearerToken, newRefreshValue);
    }
}
