using FinancialChat.Core.Interfaces;
using MediatR;

namespace FinancialChat.Core.Notifications.Handlers;

public class BotCommandReceivedNotificationHandler : INotificationHandler<BotCommandReceivedNotification>
{
    private readonly IMessageSender _messageSender;
    private readonly IAppLogger<BotCommandReceivedNotification> _logger;

    public BotCommandReceivedNotificationHandler(IMessageSender messageSender,
        IAppLogger<BotCommandReceivedNotification> logger)
    {
        _messageSender = messageSender;
        _logger = logger;
    }

    public Task Handle(BotCommandReceivedNotification notification, CancellationToken cancellationToken)
    {
        var (command, userName, groupId) = notification;

        _logger.LogInformation("Publishing command {Command} to message broker", command);

        _messageSender.Publish(
            new BotCommandReceivedNotification(command, userName, groupId));

        return Task.CompletedTask;
    }
}