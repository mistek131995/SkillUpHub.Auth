using System;
using System.Threading.Tasks;

namespace SkillUpHub.Auth.Contract.Clients;

public interface IMessageBusClient
{
    void PublishMessageAsync<T>(T message, string routingKey);
    void SubscribeAsync<T>(string queueName, Action<T> onMessageReceived);
}