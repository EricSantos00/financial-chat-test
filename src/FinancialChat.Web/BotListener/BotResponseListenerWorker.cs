using FinancialChat.Core.Interfaces;
using FinancialChat.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Web.BotListener;

public class BotResponseListenerWorker : BackgroundService
{
    private readonly IMessageReceiver<BotCommandResponse> _messageReceiver;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly ILogger<BotResponseListenerWorker> _logger;

    public BotResponseListenerWorker(IMessageReceiver<BotCommandResponse> messageReceiver,
        IHubContext<ChatHub, IChatClient> hubContext,
        ILogger<BotResponseListenerWorker> logger)
    {
        _messageReceiver = messageReceiver;
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageReceiver.Receive(async response =>
        {
            var (message, groupId) = response;

            _logger.LogInformation("Received message: {Message}", message);

            await _hubContext.Clients.Group(groupId).ReceiveMessage(DateTime.Now, "[Bot]", message);
        });

        return Task.CompletedTask;
    }
}