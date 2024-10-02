﻿using Grpc.Core;
using SkillUpHub.Auth.Contract.Services;
using IServiceProvider = SkillUpHub.Auth.Contract.Providers.IServiceProvider;

namespace SkillUpHub.Auth.Services;

public class AuthService(IServiceProvider serviceProvider) : SkillUpHub.AuthService.AuthServiceBase
{
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var token = await serviceProvider.AuthService.LoginAsync(new IAuthService.LoginUserDTO(request.Login, request.Password));
        context.ResponseTrailers.Add("Set-Cookie", $"refreshToken={token.refreshToken}; HttpOnly; Secure; SameSite=Strict; Path=/");
        
        return new LoginResponse()
        {
            AccessToken = token.accessToken,
        };
    }

    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        var userId = await serviceProvider.AuthService.CreateUserAsync(new IAuthService.CreateUserDTO(
            request.Login, 
            request.Password,
            request.Email, 
            request.Token));
        
        return new RegisterResponse()
        {
            IsSuccess = userId != default,
        };
    }

    public override async Task<RestorePasswordResponse> RestorePassword(RestorePasswordRequest request, ServerCallContext context)
    {
        return new RestorePasswordResponse()
        {
            IsSuccess = true,
        };
    }

    public override Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        return base.RefreshToken(request, context);
    }
}