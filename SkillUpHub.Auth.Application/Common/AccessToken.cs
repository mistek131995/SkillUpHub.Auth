using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SkillUpHub.Command.Application.Common;

public class AccessToken
{
    public static string GenerateAccessToken(string secretKey, string login)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new Claim("Id", Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, login),
        };

        var token = new JwtSecurityToken(
            issuer: "SkillHub.Auth",
            audience: "SkillHub.Services",
            claims: claims, expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}