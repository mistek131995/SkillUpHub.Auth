using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Infrastructure.Clients;

public sealed class RabbitMqClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IOptions<RabbitMqSettings> _options;

    public RabbitMqClient(IOptions<RabbitMqSettings> options)
    {
        var factory = new ConnectionFactory()
        {
            HostName = options.Value.Host
        };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _options = options;
    }

    public void Initialize()
    {
        if (_options.Value.Exchanges is { Count: > 0 })
        {
            foreach (var exchange in _options.Value.Exchanges)
            {
                _channel.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete);

                if (exchange.Queues is { Count: > 0 })
                {
                    foreach (var queue in exchange.Queues)
                    {
                        _channel.QueueDeclare(queue.Name, queue.Durable, queue.AutoDelete, queue.Exclusive);
                        _channel.QueueBind(queue.Name, exchange.Name, queue.Key);
                    }
                }
            }
        }

        if (_options.Value.Queues is { Count: > 0 })
        {
            foreach (var queue in _options.Value.Queues)
            {
                _channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete);
            }
        }
    }

    public void PublishMessage<T>(T message, string exchange, string routingKey)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: exchange,
            routingKey: routingKey,
            basicProperties: null,
            body: body);
    }
    
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}