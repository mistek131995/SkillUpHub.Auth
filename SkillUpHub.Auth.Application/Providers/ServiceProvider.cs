using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Auth.Application.Services;
using SkillUpHub.Auth.Contract.Services;
using IProviderService = SkillUpHub.Auth.Contract.Providers.IServiceProvider;

namespace SkillUpHub.Auth.Application.Providers;

public class ServiceProvider(IServiceProvider serviceProvider) : IProviderService
{
    private readonly Dictionary<Type, IBaseService> _services = new();
    
    private T Get<T>() where T : IBaseService
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }

        service = ActivatorUtilities.CreateInstance<T>(serviceProvider);
        
        _services[type] = service;

        return (T)service;
    }


    public IAuthService AuthService => Get<AuthService>();
}