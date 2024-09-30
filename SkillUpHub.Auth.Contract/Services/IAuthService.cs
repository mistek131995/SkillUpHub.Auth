using System;
using System.Threading.Tasks;

namespace SkillUpHub.Auth.Contract.Services
{
    public interface IAuthService : IBaseService
    {
        record UserDTO(string Login, string Password, string Email, string Token);
        Task<Guid> CreateUserAsync(UserDTO user);
    }
}