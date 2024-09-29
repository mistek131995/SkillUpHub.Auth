﻿using Grpc.Core;
using SkillUpHub;

namespace SkillUpHub.Auth.Services;

public class AuthService : SkillUpHub.AuthService.AuthServiceBase
{
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        return new LoginResponse()
        {
            Token = "Token",
        };
    }

    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        return new RegisterResponse()
        {
            IsSuccess = true,
        };
    }

    public override async Task<RestorePasswordResponse> RestorePassword(RestorePasswordRequest request, ServerCallContext context)
    {
        return new RestorePasswordResponse()
        {
            IsSuccess = true,
        };
    }
}