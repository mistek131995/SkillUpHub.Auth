using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillUpHub.Auth.Interfaces;
using CreateUser = SkillUpHub.Command.Application.Handlers.CreateUser;
using LoginIn = SkillUpHub.Command.Application.Handlers.LoginIn;

namespace SkillUpHub.Auth.Routes
{
    public class Auth : IApi
    {
        public void RegisterRoutes(WebApplication app)
        {
            app.MapPost("/CreateUser", async ([FromBody]CreateUser.Command command, IMediator mediator) =>
            {
                return await mediator.Send(command);
            });

            app.MapPost("/LoginIn", async ([FromBody]LoginIn.Command command, HttpContext context, IMediator mediator) =>
            {
                var result = await mediator.Send(command);

                return result.accessToken;
            });
        }
    }
}
