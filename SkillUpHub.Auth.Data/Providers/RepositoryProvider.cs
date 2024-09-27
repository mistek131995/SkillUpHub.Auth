using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Auth.Data.Interfaces;
using SkillUpHub.Auth.Data.Interfaces.Repositories;
using SkillUpHub.Auth.Data.Repositories;

namespace SkillUpHub.Auth.Data.Providers
{
    public class RepositoryProvider(IServiceProvider serviceProvider) : IRepositoryProvider
    {
        private readonly Dictionary<Type, IBaseRepository> _repositories = new();
        private T Get<T>() where T : IBaseRepository
        {
            var type = typeof(T);
            
            if (_repositories.TryGetValue(typeof(T), out IBaseRepository repositories))
            {
                return (T)repositories;
            }

            repositories = ActivatorUtilities.CreateInstance<T>(serviceProvider) ?? 
                           throw new Exception($"Failed to create repository {type.Name}");

            _repositories[type] = repositories;

            return (T)repositories;
        }

        public IUserRepository UserRepository => Get<UserRepository>();
    }
}