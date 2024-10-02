using System;
using System.Threading.Tasks;

namespace SkillUpHub.Auth.Contract.Services
{
    public interface IAuthService : IBaseService
    {
        record CreateUserDTO(string Login, string Password, string Email, string Token);
        Task<Guid> CreateUserAsync(CreateUserDTO user);
        
        record LoginUserDTO(string Login, string Password);
        Task<(string accessToken, string refreshToken)> LoginAsync(LoginUserDTO user);
    }
    
    
}