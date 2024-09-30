using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Providers;
using SkillUpHub.Auth.Contract.Services;

namespace SkillUpHub.Auth.Application.Services
{
    public class AuthService(IRepositoryProvider repositoryProvider) : IAuthService
    {
        public async Task<Guid> CreateUserAsync(IAuthService.UserDTO user)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(user.Login);

            if (dbUser != null)
                throw new Exception("User already exists");
            
            dbUser = await repositoryProvider.UserRepository.GetByEmailAsync(user.Email);

            if (dbUser != null)
                throw new Exception("Email already exists");
            
            dbUser = await repositoryProvider.UserRepository.SaveAsync(new User(user.Login, user.Password, user.Email));

            return dbUser.Id;
        }
    }
}