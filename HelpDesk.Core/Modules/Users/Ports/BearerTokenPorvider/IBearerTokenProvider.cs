using HelpDesk.Core.Modules.Users.Entities;
using HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider.DTOs;
using ROP;

namespace HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider;

public interface IBearerTokenProvider
{
    Task<BearerTokenGenerated> GenerateToken(AppUser user);
    string GenerateRefreshToken();
}
