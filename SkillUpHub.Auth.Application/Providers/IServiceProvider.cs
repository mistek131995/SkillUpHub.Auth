using SkillUpHub.Auth.Contract.Services;

namespace SkillUpHub.Auth.Application.Providers;

public interface IServiceProvider
{
    IAuthService AuthService { get; }
}