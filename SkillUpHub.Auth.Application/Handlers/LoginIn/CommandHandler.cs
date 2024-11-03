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
            var user = await repositoryProvider.UserRepository.GetByLoginAsync(request.Login) ??
                         throw new HandledException("Пользователь с таким логином и паролем не найден");

            if (user.IsPasswordValid(request.Password))
            {
                var accessToken = AccessToken.GenerateAccessToken(configuration.GetSection("SecretKey").Value!, user);
                var refreshToken = Contract.Models.RefreshToken.GenerateRefreshToken();

                var userRefreshTokens = await repositoryProvider.RefreshTokenRepository.GetByUserIdAsync(user.Id);
                var curToken = userRefreshTokens
                    .FirstOrDefault(x => x.UserAgent == request.UserAgent && x.Fingerprint == request.FingerPrint);

                if (curToken != null)
                    curToken.Update(refreshToken);
                else
                    curToken = new Contract.Models.RefreshToken(refreshToken, request.FingerPrint, request.UserAgent, user.Id);

                await repositoryProvider.RefreshTokenRepository.SaveAsync(curToken);
                
                httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken);

                return accessToken;
            }

            throw new HandledException("Пользователь с таким логином и паролем не найден.");
        }
    }
}
