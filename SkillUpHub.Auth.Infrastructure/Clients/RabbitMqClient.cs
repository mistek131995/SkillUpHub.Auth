using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillUpHub.Auth.Contract.Clients;

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
    
    public void PublishMessageAsync<T>(T message, string routingKey)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: "",
            routingKey: routingKey,
            basicProperties: null,
            body: body);
    }

    public void SubscribeAsync<T>(string queueName, Action<T> onMessageReceived)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonConvert.DeserializeObject<T>(message);

            onMessageReceived(deserializedMessage);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }
    
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}