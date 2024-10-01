using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Providers;
using SkillUpHub.Auth.Contract.Services;

namespace SkillUpHub.Auth.Application.Services
{
    public class AuthService(IRepositoryProvider repositoryProvider) : IAuthService
    {
        public async Task<Guid> CreateUserAsync(IAuthService.CreateUserDTO user)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(user.Login);

            if (dbUser != null)
                throw new Exception("Пользователь с таким логином уже зарегистрирован.");
            
            dbUser = await repositoryProvider.UserRepository.GetByEmailAsync(user.Email);

            if (dbUser != null)
                throw new Exception("Пользователь с таким адресом электронной почты уже зарегистрирован.");
            
            //BCrypt.Verify("my password", passwordHash);
            dbUser = await repositoryProvider.UserRepository.SaveAsync(new User(user.Login, BCrypt.Net.BCrypt.HashPassword(user.Password), user.Email));

            return dbUser.Id;
        }

        public async Task<string> LoginAsync(IAuthService.LoginUserDTO user)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(user.Login) ??
                         throw new Exception("Пользователь с таким логином и паролем не найден");

            if (BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password))
            {
                return "Token";
            }

            throw new Exception("Пользователь с таким логином и паролем не найден.");
        }
    }
}