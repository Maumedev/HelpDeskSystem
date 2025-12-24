
namespace HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider.DTOs;

public record BearerTokenGenerated
(
    string BearerToken,
    string TokenId
    );
