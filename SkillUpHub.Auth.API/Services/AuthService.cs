using Grpc.Core;
using SkillUpHub.AuthService;

namespace SkillUpHub.Auth.Services;

public class AuthService : SkillUpHub.AuthService.AuthService.AuthServiceBase
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
}