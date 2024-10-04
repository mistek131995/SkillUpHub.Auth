using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Auth.Data.Interfaces;
using SkillUpHub.Auth.Contract.Providers;
using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Auth.Infrastructure.Repositories;
using IServiceProvider = System.IServiceProvider;

namespace SkillUpHub.Auth.Infrastructure.Providers
{
    public class RepositoryProvider(IServiceProvider serviceProvider) : IRepositoryProvider
    {
        private readonly Dictionary<Type, IBaseRepository> _repositories = new();
        private T Get<T>() where T : IBaseRepository
        {
            var type = typeof(T);
            
            if (_repositories.TryGetValue(type, out IBaseRepository repositories))
            {
                return (T)repositories;
            }

            repositories = ActivatorUtilities.CreateInstance<T>(serviceProvider) ?? 
                           throw new Exception($"Failed to create repository {type.Name}");

            _repositories[type] = repositories;

            return (T)repositories;
        }

        public IUserRepository UserRepository => Get<UserRepository>();
        public IRefreshTokenRepository RefreshTokenRepository => Get<RefreshTokenRepository>();
    }
}