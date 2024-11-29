using System;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Infrastructure.Interfaces;

public interface IMessageBusClient
{
    Task Initialize();
    Task PublishMessage<T>(T message, string exchange, string routingKey);
    Task PublishErrorMessage(Exception exception);
    Task Subscribe<T>(string queueName, Func<T, Task> onMessageReceived);
}