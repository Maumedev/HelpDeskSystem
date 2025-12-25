using HelpDesk.Core.Modules.Users.Entities;
using HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider;
using HelpDesk.Core.Modules.Users.Ports.BearerTokenPorvider.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelpDesk.Infrastructure.Services;

public class BearerTokenProvider(
    IConfiguration configuration,
    UserManager<AppUser> userManager) : IBearerTokenProvider
{

    public async Task<BearerTokenGenerated> GenerateToken(AppUser user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        ArgumentNullException.ThrowIfNull(jwtSettings);

        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

        var jti = Guid.NewGuid().ToString();
        // 1. Definimos los Claims (La info dentro del token)
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, jti),
            new(ClaimTypes.NameIdentifier, user.Id), // ID del Usuario
            new(ClaimTypes.Email, user.Email),
            new("FullName", user.FullName ?? ""),   // Claim personalizado
            new("Department", user.Departament ?? "") // Claim personalizado
        };

        // 2. Obtenemos los roles y los agregamos como claims
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSigningKey = new SymmetricSecurityKey(key);


        var jwt = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"])), // El token expira en 3 horas
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        // 3. Creamos el token
        //var tokenDescriptor = new JwtSecurityToken
        //{
        //    Subject = new ClaimsIdentity(claims),
        //    Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"])),
        //    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
        //    Issuer = jwtSettings["Issuer"],
        //    Audience = jwtSettings["Audience"]
        //};

        var bearerToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        var tokenId = jti;

        return new BearerTokenGenerated(bearerToken, tokenId);
        
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
