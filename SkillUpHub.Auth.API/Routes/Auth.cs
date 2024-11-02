using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillUpHub.Auth.Interfaces;
using CreateUser = SkillUpHub.Command.Application.Handlers.CreateUser;
using LoginIn = SkillUpHub.Command.Application.Handlers.LoginIn;
using RefreshToken = SkillUpHub.Command.Application.Handlers.RefreshToken;

namespace SkillUpHub.Auth.Routes
{
    public class Auth : IApi
    {
        public void RegisterRoutes(WebApplication app)
        {
            app.MapPost("/CreateUser", async ([FromBody]CreateUser.Command command, IMediator mediator) => await mediator.Send(command));

            app.MapPost("/LoginIn", async ([FromBody]LoginIn.Command command, IMediator mediator) => await mediator.Send(command));

            app.MapPost("/RefreshToken", async ([FromBody]RefreshToken.Command command, IMediator mediator) => await mediator.Send(command));
        }
    }
}
