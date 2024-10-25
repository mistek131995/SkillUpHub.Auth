using SkillUpHub.Auth.Interfaces;

namespace SkillUpHub.Auth.Extensions
{
    public static class RoutesRegistrationExtension
    {
        public static void RegisterRoutes(this WebApplication app, IServiceProvider serviceProvider)
        {
            var apiTypes = typeof(Program).Assembly.GetTypes()
                .Where(t => typeof(IApi).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in apiTypes)
            {
                //var apiInstance = Activator.CreateInstance(type) as IApi;

                var apiInstance = ActivatorUtilities.CreateInstance(serviceProvider, type) as IApi;

                apiInstance?.RegisterRoutes(app);
            }
        }
    }
}
