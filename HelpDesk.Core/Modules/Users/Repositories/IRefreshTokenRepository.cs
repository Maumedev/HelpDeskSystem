using HelpDesk.Core.Modules.Users.Entities;

namespace HelpDesk.Core.Modules.Users.Repositories;

public interface IRefreshTokenRepository
{
    Task Add(RefreshToken refreshToken);
    Task Update(RefreshToken token);
    Task<RefreshToken?> GetByTokenId(string token);
    Task RevokeActiveTokens(string userId);
}
