namespace SkillUpHub.Auth.Application.Interfaces;

public interface IServiceProvider
{
    IAuthService AuthService { get; }
}