using HelpDesk.Core.Modules.Users.Entities;
using HelpDesk.Core.Modules.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(
    HelpDeskContext context) : IRefreshTokenRepository
{
    public async Task Add(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenId(string token)
    {
        var tokenToReturn = await context.RefreshTokens.Where(t => t.Token == token).FirstOrDefaultAsync();
        return tokenToReturn;
    }

    public async Task RevokeActiveTokens(string userId)
    {
        var tokensActives = context.RefreshTokens
        .Where(x => x.UserId == userId && !x.IsRevoked && !x.IsUsed)
        .ToList();

        tokensActives.ForEach(t => t.Revoke());

        await context.SaveChangesAsync();
    }

    public async Task Update(RefreshToken token)
    {
        context.RefreshTokens.Update(token);
        await context.SaveChangesAsync();
    }
}
