namespace SkillUpHub.Command.Infrastructure.Interfaces;

public interface IMessageBusClient
{
    void Initialize();
    void PublishMessage<T>(T message, string exchange, string routingKey);
    //void SubscribeAsync<T>(string queueName, Action<T> onMessageReceived);
}