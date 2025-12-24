using HelpDesk.Core.Shared.Exceptions;
using ROP;

namespace HelpDesk.Core.Modules.Users.Entities;

public class RefreshToken
{
    public int Id { get; private set; }
    public string Token { get; private set; } 
    public string JwtId { get; private set; } // Para enlazarlo al JWT específico
    public DateTime AddedDate { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; private set; } = DateTime.UtcNow.AddDays(7);
    public bool IsRevoked { get; private set; } = false;
    public bool IsUsed { get; private set; } = false;
    public string UserId { get; private set; }


    private RefreshToken( string token, string jwtId, string userId   )
    {
        Token = token;
        JwtId = jwtId;
        UserId = userId;
    }

    public RefreshToken()
    {
    }

    public static RefreshToken Create(string token, string jwtId, string userId)
    {
        return new RefreshToken( token, jwtId,  userId );
    }

    public Result<Unit> MarkAsUsed()
    {
        List<Error> errors = [];

        if (DateTime.UtcNow >= ExpiryDate)
            errors.Add(Error.Create("El refresh token ha expirado"));

        if (IsRevoked)
            errors.Add(Error.Create("Refresh token a sido revocado"));

        if (IsUsed)
            errors.Add(Error.Create("Refresh token a sido usado"));

        if (errors.Count > 0) return Result.Failure(errors);

        IsUsed = true;
        return Result.Success();
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
