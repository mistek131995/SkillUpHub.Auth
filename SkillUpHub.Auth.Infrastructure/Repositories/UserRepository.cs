using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Mapping;
using EUser = SkillUpHub.Auth.Infrastructure.Entities.User;

namespace SkillUpHub.Auth.Infrastructure.Repositories
{
    public class UserRepository(PGContext context) : IUserRepository
    {
        public async Task<User> GetByIdAsync(Guid id)
        {
            var mapper = new UserMapper();
            var dbUser = await context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return mapper.MappingToContractModel(dbUser);
        }

        public async Task<List<User>> GetByIdsAsync(List<Guid> ids)
        {
            var mapper = new UserMapper();
            var dbUsers = await context.Users
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            
            return mapper.MappingToContractModel(dbUsers);
        }

        
        public async Task<User> GetByEmailAsync(string email)
        {
            var mapper = new UserMapper();
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);

            return mapper.MappingToContractModel(user);
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            var mapper = new UserMapper();
            var user = await context.Users.FirstOrDefaultAsync(x => x.Login == login);

            return mapper.MappingToContractModel(user);
        }

        public async Task<User> SaveAsync(User user)
        {
            var mapper = new UserMapper();
            var dbUser = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            
            if (dbUser == null)
            {
                dbUser = mapper.MappingToInfrastructureModel(user);
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