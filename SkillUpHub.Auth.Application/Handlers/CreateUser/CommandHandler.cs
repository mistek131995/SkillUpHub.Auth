using MediatR;
using Microsoft.Extensions.Options;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Command.Application.Exceptions;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    public class CommandHandler(IRepositoryProvider repositoryProvider, IMessageBusClient messageBusClient) : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(request.Login);

            if (dbUser != null)
                throw new HandledException("Пользователь с таким логином уже зарегистрирован.");

            dbUser = await repositoryProvider.UserRepository.GetByEmailAsync(request.Email);

            if (dbUser != null)
                throw new HandledException("Пользователь с таким адресом электронной почты уже зарегистрирован.");

            dbUser = await repositoryProvider.UserRepository.SaveAsync(new User(request.Login, request.Password, request.Email));

            messageBusClient.PublishMessage(new
            {
                UserId = dbUser.Id,
                SessionId = request.SessionId,
            }, "create-user", "create-user.account-create-success");

            return Unit.Value;
        }
    }
}
