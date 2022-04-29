using FinancialChat.Bot.MessageBroker;
using FinancialChat.Bot.Messages;
using FinancialChat.Bot.Services;
using Microsoft.Extensions.Hosting;

namespace FinancialChat.Bot;

public class ChatService : BackgroundService
{
    private readonly IMessageReceiver<BotCommandMessage> _botCommandMessageReceiver;
    private readonly BotService _botService;

    public ChatService(IMessageReceiver<BotCommandMessage> botCommandMessageReceiver, BotService botService)
    {
        _botCommandMessageReceiver = botCommandMessageReceiver;
        _botService = botService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _botCommandMessageReceiver.Receive(async (command) =>
        {
            Console.WriteLine($"Received command: {command.Command}");

            await _botService.ProcessCommand(command);
        });

        return Task.CompletedTask;
    }
}