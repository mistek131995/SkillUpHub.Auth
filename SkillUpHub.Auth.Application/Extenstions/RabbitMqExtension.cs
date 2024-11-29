using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Clients;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Application.Extenstions;

public static class RabbitMqExtension
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        services.AddScoped<IMessageBusClient, RabbitMqClient>();
        
        return services;
    }
}