using System.Text;
using System.Text.Json;
using FinancialChat.Core.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialChat.Infrastructure.RabbitMQ;

public class RabbitMQReceiver<T> : IMessageReceiver<T>, IDisposable
{
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    private readonly RabbitMQReceiverOptions _options;

    public RabbitMQReceiver(RabbitMQReceiverOptions options)
    {
        _options = options;

        _connection = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            AutomaticRecoveryEnabled = true,
        }.CreateConnection();

        _queueName = _options.QueueName;

        _connection.ConnectionShutdown += Connection_ConnectionShutdown;
    }

    private static void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
    }

    public void Receive(Func<T, Task> action)
    {
        _channel = _connection.CreateModel();

        if (_options.AutomaticCreateEnabled)
        {
            _channel.QueueDeclare(_options.QueueName, true, false, false, null);
            _channel.QueueBind(_options.QueueName, _options.ExchangeName, _options.RoutingKey, null);
        }

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.Span);
            var message = JsonSerializer.Deserialize<T>(body);

            if (message != null)
                action(message);

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: _queueName,
            autoAck: false,
            consumer: consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}