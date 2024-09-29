using SkillUpHub.Auth.Domain.Providers;
using SkillUpHub.Auth.Domain.Services;

namespace SkillUpHub.Auth.Application.Services
{
    public class AuthService(IRepositoryProvider repositoryProvider) : IAuthService
    {
    }
}