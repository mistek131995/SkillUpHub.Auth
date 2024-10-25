﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Interfaces;
using SkillUpHub.Auth.Infrastructure.Providers;

namespace SkillUpHub.Command.Application
{
    public static class CommandService
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PGContext>(option => option.UseNpgsql(connectionString));
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CommandService).Assembly));

            return services;
        }
    }
}