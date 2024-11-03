using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SkillUpHub.Auth.Contract.Models;

namespace SkillUpHub.Command.Application.Common;

public class AccessToken
{
    public static string GenerateAccessToken(string secretKey, User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
        };

        var token = new JwtSecurityToken(
            issuer: "SkillHub.Auth",
            audience: "SkillHub.Services",
            claims: claims, expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}