﻿using SkillUpHub.Auth.Interfaces;

namespace SkillUpHub.Auth.Extensions
{
    public static class RoutesRegistrationExtension
    {
        public static void RegisterRoutes(this WebApplication app)
        {
            var apiTypes = typeof(Program).Assembly.GetTypes()
                .Where(t => typeof(IApi).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            using var scope = app.Services.CreateScope();
            foreach (var type in apiTypes)
            {
                var apiInstance = Activator.CreateInstance(type) as IApi;
                apiInstance?.RegisterRoutes(app);
            }
        }
    }
}
