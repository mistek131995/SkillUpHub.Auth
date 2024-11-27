using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SkillUpHub.Command.Application.Common;
using SkillUpHub.Command.Application.Exceptions;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Application.Handlers.RefreshToken;

public class CommandHandler(IRepositoryProvider repositoryProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IRequestHandler<Command, string>
{
    public async Task<string> Handle(Command request, CancellationToken cancellationToken)
    {
        var refreshTokenCookie = httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshTokenCookie))
            throw new RefreshTokenException("Время вашей сессии истекло. Пожалуйста, войдите в свой аккаунт заново, чтобы продолжить работу");

        var refreshToken = await repositoryProvider.RefreshTokenRepository.GetByToken(refreshTokenCookie) ?? 
            throw new RefreshTokenException("Время вашей сессии истекло. Пожалуйста, войдите в свой аккаунт заново, чтобы продолжить работу");

        if(refreshToken.TokenIsValid(refreshTokenCookie, request.FingerPrint, request.UserAgent))
        {
            var user = await repositoryProvider.UserRepository.GetByIdAsync(refreshToken.UserId);

            refreshToken.Token = Contract.Models.RefreshToken.GenerateRefreshToken();
            await repositoryProvider.RefreshTokenRepository.SaveAsync(refreshToken);
            httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken.Token);
            
            return AccessToken.GenerateAccessToken(configuration.GetSection("SecretKey").Value!, user);
        }
        
        throw new RefreshTokenException("Время вашей сессии истекло. Пожалуйста, войдите в свой аккаунт заново, чтобы продолжить работу");
    }
}