using Newtonsoft.Json;
using Note.Producer.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace Note.Producer;

public class Producer : IMessageProducer
{
    public void SendMessage<T>(T message, string routingKey, string? exchange = null)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        var messageJson = JsonConvert.SerializeObject(message, Formatting.Indented,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

        var messageBody = Encoding.UTF8.GetBytes(messageJson);

        channel.BasicPublish(exchange, routingKey, body: messageBody);
    }
}
