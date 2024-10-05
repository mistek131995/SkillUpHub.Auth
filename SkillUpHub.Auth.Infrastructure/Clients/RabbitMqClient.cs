using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillUpHub.Auth.Infrastructure.Interfaces;

namespace SkillUpHub.Auth.Infrastructure.Clients;

public sealed class RabbitMqClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqClient()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public void PublishMessage<T>(T message, string routingKey)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: "",
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