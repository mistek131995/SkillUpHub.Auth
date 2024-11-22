using MediatR;
using Microsoft.Extensions.Options;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Infrastructure.Interfaces;
using SkillUpHub.Command.Application.Exceptions;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    public class CommandHandler(IRepositoryProvider repositoryProvider, IMessageBusClient messageBusClient, IOptions<RabbitMqSettings> options) : IRequestHandler<Command, Unit>
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
            
            var queue = options.Value.Queues.FirstOrDefault(x => x.Id == "create-account-complete") 
                        ?? throw new NullReferenceException("Очередь успешного создания аккаунта не найдена");

            messageBusClient.PublishMessage(new
            {
                UserId = dbUser.Id,
                SessionId = request.SessionId,
            }, "", queue.Name);

            return Unit.Value;
        }
    }
}
