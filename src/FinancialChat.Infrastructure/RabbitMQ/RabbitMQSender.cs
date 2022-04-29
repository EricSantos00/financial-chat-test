using System.Text.Json;
using FinancialChat.Core.Interfaces;
using FinancialChat.Core.Notifications;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FinancialChat.Infrastructure.RabbitMQ;

public class RabbitMQSender : IMessageSender
{
    private readonly RabbitMQSenderOptions _senderOptions;

    public RabbitMQSender(IOptions<RabbitMQSenderOptions> settings)
    {
        _senderOptions = settings.Value;
    }

    public void Publish(NotificationBase notification)
    {
        var factory = new ConnectionFactory
        {
            HostName = _senderOptions.HostName,
            UserName = _senderOptions.UserName,
            Password = _senderOptions.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        var body = JsonSerializer.SerializeToUtf8Bytes(notification, notification.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });

        channel.BasicPublish(
            exchange: _senderOptions.ExchangeName,
            routingKey: _senderOptions.RoutingKey,
            basicProperties: null,
            body: body);
    }
}