using MediatR;
using Microsoft.Extensions.Configuration;
using SkillUpHub.Auth.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using SkillUpHub.Command.Application.Common;
using SkillUpHub.Command.Application.Exceptions;

namespace SkillUpHub.Command.Application.Handlers.LoginIn
{
    internal class CommandHandler(IRepositoryProvider repositoryProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(request.Login) ??
                         throw new HandledException("Пользователь с таким логином и паролем не найден");

            if (dbUser.IsPasswordValid(request.Password))
            {
                var accessToken = AccessToken.GenerateAccessToken(configuration.GetSection("SecretKey").Value!, request.Login);
                var refreshToken = Contract.Models.RefreshToken.GenerateRefreshToken();

                var userRefreshTokens = await repositoryProvider.RefreshTokenRepository.GetByUserIdAsync(dbUser.Id);
                var curToken = userRefreshTokens
                    .FirstOrDefault(x => x.UserAgent == request.UserAgent && x.Fingerprint == request.FingerPrint);

                if (curToken != null)
                    curToken.Update(refreshToken);
                else
                    curToken = new Contract.Models.RefreshToken(refreshToken, request.FingerPrint, request.UserAgent, dbUser.Id);

                await repositoryProvider.RefreshTokenRepository.SaveAsync(curToken);
                
                httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken);

                return accessToken;
            }

            throw new HandledException("Пользователь с таким логином и паролем не найден.");
        }
    }
}
