using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Mappers;
using SkillUpHub.Command.Contract.Repositories;

namespace SkillUpHub.Command.Infrastructure.Repositories
{
    public class UserRepository(PGContext context) : IUserRepository
    {
        public async Task<User> GetByIdAsync(Guid id)
        {
            var mapper = new UserMapper();
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return user == null ? null : mapper.MappingToContractModel(user);
        }

        public async Task<List<User>> GetByIdsAsync(List<Guid> ids)
        {
            var mapper = new UserMapper();
            var users = await context.Users
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            
            return mapper.MappingToContractModel(users);
        }

        
        public async Task<User> GetByEmailAsync(string email)
        {
            var mapper = new UserMapper();
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            return user == null ? null : mapper.MappingToContractModel(user);
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            var mapper = new UserMapper();
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login);

            return user == null ? null : mapper.MappingToContractModel(user);
        }

        public async Task<User> SaveAsync(User user)
        {
            var mapper = new UserMapper();
            var dbUser = mapper.MappingToInfrastructureModel(user);

            var existingUser = await context.Users.FindAsync(dbUser.Id);
            
            if (existingUser != null)
                context.Entry(existingUser).CurrentValues.SetValues(dbUser);
            else
                context.Users.Add(dbUser);
            
            await context.SaveChangesAsync();
            
            return new User(dbUser.Id, dbUser.Login, dbUser.Password, dbUser.Email);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}