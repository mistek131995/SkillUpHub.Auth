using System;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Infrastructure.Interfaces;

public interface IRabbitMqMessageHandler
{
    Task RollbackAccountAsync(Guid id);
}