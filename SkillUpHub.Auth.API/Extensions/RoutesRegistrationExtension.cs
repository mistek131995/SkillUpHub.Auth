﻿using SkillUpHub.Auth.Interfaces;

namespace SkillUpHub.Auth.Extensions
{
    public static class RoutesRegistrationExtension
    {
        public static void RegisterRoutes(this WebApplication app)
        {
            var apiTypes = typeof(Program).Assembly.GetTypes()
                .Where(t => typeof(IApi).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in apiTypes)
            {
                var apiInstance = ActivatorUtilities.CreateInstance(app.Services, type) as IApi;

                apiInstance?.RegisterRoutes(app);
            }
        }
    }
}
