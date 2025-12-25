
namespace HelpDesk.Core.Modules.Users.Shared.DTOs;

public record LoginResponse(string BearerToken, string RefreshToken);