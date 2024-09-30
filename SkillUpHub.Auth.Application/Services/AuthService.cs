using SkillUpHub.Auth.Contract.Providers;
using SkillUpHub.Auth.Contract.Services;

namespace SkillUpHub.Auth.Application.Services
{
    public class AuthService(IRepositoryProvider repositoryProvider) : IAuthService
    {
    }
}