using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Providers;
using SkillUpHub.Auth.Contract.Services;

namespace SkillUpHub.Auth.Application.Services
{
    public class AuthService(IRepositoryProvider repositoryProvider, IConfiguration configuration) : IAuthService
    {
        public async Task<Guid> CreateUserAsync(IAuthService.CreateUserDTO user)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(user.Login);

            if (dbUser != null)
                throw new Exception("Пользователь с таким логином уже зарегистрирован.");
            
            dbUser = await repositoryProvider.UserRepository.GetByEmailAsync(user.Email);

            if (dbUser != null)
                throw new Exception("Пользователь с таким адресом электронной почты уже зарегистрирован.");
            
            //BCrypt.Verify("my password", passwordHash);
            dbUser = await repositoryProvider.UserRepository.SaveAsync(new User(user.Login, BCrypt.Net.BCrypt.HashPassword(user.Password), user.Email));

            return dbUser.Id;
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(IAuthService.LoginUserDTO user)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(user.Login) ??
                         throw new Exception("Пользователь с таким логином и паролем не найден");

            if (BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password))
            {
                var accessToken = GenerateAccessToken(user.Login);
                var refreshToken = GenerateRefreshToken();
                
                var userRefreshTokens = await repositoryProvider.RefreshTokenRepository.GetByUserIdAsync(dbUser.Id);
                var curToken = userRefreshTokens
                    .FirstOrDefault(x => x.UserAgent == user.UserAgent && x.Fingerprint == user.FingerPrint);

                if (curToken != null)
                    curToken.Update(refreshToken);
                else
                    curToken = new RefreshToken(refreshToken, user.FingerPrint, user.UserAgent, dbUser.Id);

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