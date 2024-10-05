using System;

namespace SkillUpHub.Auth.Infrastructure.Interfaces;

public interface IMessageBusClient
{
    void PublishMessage<T>(T message, string routingKey);
    //void SubscribeAsync<T>(string queueName, Action<T> onMessageReceived);
}