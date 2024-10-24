using SkillUpHub.Auth.Interfaces;

namespace SkillUpHub.Auth.Routes
{
    public class Auth : IApi
    {
        public void RegisterRoutes(WebApplication app)
        {
            app.MapPost("/Register", () =>
            {

            });
        }
    }
}
