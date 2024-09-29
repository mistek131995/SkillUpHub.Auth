using SkillUpHub.Auth.Core.Interfaces.Services;

namespace SkillUpHub.Auth.Core.Interfaces;

public interface ICoreServiceProvider
{
    IAuthService AuthService { get; }
}