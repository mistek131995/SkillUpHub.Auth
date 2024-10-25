using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillUpHub.Auth.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SkillUpHub.Auth.Contract.Models;

namespace SkillUpHub.Command.Application.Handlers.LoginIn
{
    internal class CommandHandler(IRepositoryProvider repositoryProvider, IConfiguration configuration) : IRequestHandler<Command, (string accessToken, string refreshToken)>
    {
        public async Task<(string accessToken, string refreshToken)> Handle(Command request, CancellationToken cancellationToken)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(request.Login) ??
                         throw new Exception("Пользователь с таким логином и паролем не найден");

            if (dbUser.IsPasswordValid(request.Password))
            {
                var accessToken = GenerateAccessToken(request.Login);
                var refreshToken = GenerateRefreshToken();

                var userRefreshTokens = await repositoryProvider.RefreshTokenRepository.GetByUserIdAsync(dbUser.Id);
                var curToken = userRefreshTokens
                    .FirstOrDefault(x => x.UserAgent == request.UserAgent && x.Fingerprint == request.FingerPrint);

                if (curToken != null)
                    curToken.Update(refreshToken);
                else
                    curToken = new RefreshToken(refreshToken, request.FingerPrint, request.UserAgent, dbUser.Id);

                await repositoryProvider.RefreshTokenRepository.SaveAsync(curToken);

                return (accessToken, refreshToken);
            }

            throw new Exception("Пользователь с таким логином и паролем не найден.");
        }

        private string GenerateAccessToken(string login)
        {
            var key = configuration.GetSection("SecretKey").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, login),
                new Claim("Id", Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "SkillHub.Auth",
                audience: "SkillHub.Services",
                claims: claims, expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(int length = 32)
        {
            var randomNumber = new byte[length];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
