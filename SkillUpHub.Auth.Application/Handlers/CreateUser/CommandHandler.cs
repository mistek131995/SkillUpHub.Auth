using MediatR;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    public class CommandHandler(IRepositoryProvider repositoryProvider, IMessageBusClient messageBusClient) : IRequestHandler<Command, Guid>
    {
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var dbUser = await repositoryProvider.UserRepository.GetByLoginAsync(request.Login);

            if (dbUser != null)
                throw new Exception("Пользователь с таким логином уже зарегистрирован.");

            dbUser = await repositoryProvider.UserRepository.GetByEmailAsync(request.Email);

            if (dbUser != null)
                throw new Exception("Пользователь с таким адресом электронной почты уже зарегистрирован.");

            dbUser = await repositoryProvider.UserRepository.SaveAsync(new User(request.Login, request.Password, request.Email));

            messageBusClient.PublishMessage(dbUser.Id, "createUser");

            return dbUser.Id;
        }
    }
}
