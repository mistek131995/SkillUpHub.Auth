using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Auth.Core.Interfaces;
using SkillUpHub.Auth.Core.Interfaces.Services;
using SkillUpHub.Auth.Core.Services;

namespace SkillUpHub.Auth.Core.Providers;

public class CoreServiceProvider(IServiceProvider serviceProvider) : ICoreServiceProvider
{
    private readonly Dictionary<Type, IBaseService> _services = new();
    
    private T Get<T>() where T : IBaseService
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out IBaseService service))
        {
            return (T)service;
        }

        service = ActivatorUtilities.CreateInstance<T>(serviceProvider);
        
        _services[type] = service;

        return (T)service;
    }

    public IAuthService AuthService => Get<AuthService>();
}