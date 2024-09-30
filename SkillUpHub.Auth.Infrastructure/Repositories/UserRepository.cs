using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Auth.Infrastructure.Contexts;
using EUser = SkillUpHub.Auth.Infrastructure.Entities.User;

namespace SkillUpHub.Auth.Infrastructure.Repositories
{
    public class UserRepository(PGContext context) : IUserRepository
    {
        public async Task<User> GetByIdAsync(Guid id)
        {
            return await context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new User(x.Id, x.Login, x.Password, x.Email))
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetByIdsAsync(List<Guid> ids)
        {
            return await context.Users
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Select(x => new User(x.Id, x.Login, x.Password, x.Email))
                .ToListAsync();
        }

        
        public async Task<User> GetByEmailAsync(string email)
        {
            var userId = (await context.Users.FirstOrDefaultAsync(x => x.Email == email))?.Id;
            
            if (userId == null)
                return null;

            return await GetByIdAsync(userId.Value);
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            var userId = (await context.Users.FirstOrDefaultAsync(x => x.Login == login))?.Id;
            
            if (userId == null)
                return null;

            return await GetByIdAsync(userId.Value);
        }

        public async Task<User> SaveAsync(User user)
        {
            var dbUser = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            
            if (dbUser == null)
            {
                dbUser = new EUser()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email,
                    Password = user.Password,
                };
                
                context.Add(dbUser);
            }
            else
            {
                dbUser.Login = user.Login;
                dbUser.Password = user.Password;
                dbUser.Email = user.Email;
            }
            
            await context.SaveChangesAsync();
            
            return new User(dbUser.Id, dbUser.Login, dbUser.Password, dbUser.Email);
        }
    }
}