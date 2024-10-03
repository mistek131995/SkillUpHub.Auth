using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SkillUpHub.Auth.Contract.Services;
using IServiceProvider = SkillUpHub.Auth.Contract.Providers.IServiceProvider;

namespace SkillUpHub.Auth.Services;

public class AuthService(IServiceProvider serviceProvider) : SkillUpHub.AuthService.AuthServiceBase
{
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        var fingerprint = httpContext.Request.Headers["Fingerprint"].ToString();
        var token = await serviceProvider.AuthService.LoginAsync(new IAuthService.LoginUserDTO(request.Login, request.Password, userAgent, fingerprint));

        // Устанавливаем Cookie в заголовке Set-Cookie
        httpContext.Response.Cookies.Append("refreshToken", token.refreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = false,
            Path = "/"
        });
        
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
    
    public override async Task<RefreshTokenResponse> RefreshToken(Empty request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        var fingerprint = httpContext.Request.Headers["Fingerprint"].ToString();
        
        var accessToken = await serviceProvider.AuthService.RefreshAccessToken(new IAuthService.RefreshTokenDTO(
            refreshToken, 
            userAgent, 
            fingerprint));

        return new RefreshTokenResponse()
        {
            AccessToken = accessToken,
        };
    }
}