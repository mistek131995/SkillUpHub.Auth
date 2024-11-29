using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Infrastructure.Clients;

public sealed class RabbitMqClient : IMessageBusClient
{
    private readonly IOptions<RabbitMqSettings> _options;

    public RabbitMqClient(IOptions<RabbitMqSettings> options)
    {
        _options = options;
    }

    public async Task Initialize()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.Host
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        if (_options.Value.Exchanges is { Count: > 0 })
        {
            foreach (var exchange in _options.Value.Exchanges)
            {
                await channel.ExchangeDeclareAsync(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete);

                if (exchange.Queues is { Count: > 0 })
                {
                    foreach (var queue in exchange.Queues)
                    {
                        await channel.QueueDeclareAsync(queue.Name, queue.Durable, queue.AutoDelete, queue.Exclusive);
                        await channel.QueueBindAsync(queue.Name, exchange.Name, queue.Key);
                    }
                }
            }
        }

        if (_options.Value.Queues is { Count: > 0 })
        {
            foreach (var queue in _options.Value.Queues)
            {
                await channel.QueueDeclareAsync(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete);
            }
        }
    }

    public async Task PublishMessage<T>(T message, string exchange, string routingKey)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.Host
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        
        await channel.BasicPublishAsync(exchange: exchange,
            routingKey: routingKey,
            body: body);
    }
    
    public async Task PublishErrorMessage(Exception exception)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.Host
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync("logger", durable: true, exclusive: false, autoDelete: false, arguments: null);
        var jsonMessage = JsonConvert.SerializeObject(exception);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync(exchange: "",
            routingKey: "logger",
            body: body);
    }

    public async Task Subscribe<T>(string queueName, Func<T, Task> onMessageReceived)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.Host
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonConvert.DeserializeObject<T>(message);
            onMessageReceived(deserializedMessage);
            
            return Task.CompletedTask;
        };
        
        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }
}