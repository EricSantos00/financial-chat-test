namespace FinancialChat.Bot.MessageBroker;

public class RabbitMQReceiverOptions
{
    public string HostName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string QueueName { get; set; } = null!;
    public bool AutomaticCreateEnabled { get; set; }
    public string ExchangeName { get; set; } = null!;
    public string RoutingKey { get; set; } = null!;
}