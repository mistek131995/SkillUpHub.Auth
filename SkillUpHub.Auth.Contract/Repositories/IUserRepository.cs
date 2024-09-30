using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Data.Interfaces;

namespace SkillUpHub.Auth.Contract.Repositories
{
    public interface IUserRepository : IBaseRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<List<User>> GetByIdsAsync(List<Guid> ids);
        
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);
        
        
        Task<User> SaveAsync(User user);
    }
}