﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Command.Application.Behaviors;
using SkillUpHub.Command.Application.Extenstions;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SkillUpHub.Command.Infrastructure.Providers;

namespace SkillUpHub.Command.Application
{
    public static class CommandService
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMq(configuration);
            
            services.AddDbContext<PGContext>(option => 
                option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(CommandService).Assembly));
            
            services.AddValidatorsFromAssembly(typeof(CommandService).Assembly);
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
