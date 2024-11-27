using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Application.MessageHandlers;

public class RabbitMqMessageHandler(IRepositoryProvider repositoryProvider) : IRabbitMqMessageHandler
{
    public async Task RollbackAccountAsync(Guid id)
    {
        await repositoryProvider.UserRepository.DeleteAsync(id);
    }
}