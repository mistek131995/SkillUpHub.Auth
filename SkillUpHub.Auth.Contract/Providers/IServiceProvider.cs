using SkillUpHub.Auth.Contract.Services;
using SkillUpHub.Auth.Contract.Providers;

namespace SkillUpHub.Auth.Contract.Providers;

public interface IServiceProvider
{
    IAuthService AuthService { get; }
}