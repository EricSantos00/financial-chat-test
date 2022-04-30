using FinancialChat.Bot.BotCommandHandler;
using FinancialChat.Bot.MessageBroker;
using FinancialChat.Bot.Messages;

namespace FinancialChat.Bot.Services;

public class BotService
{
    private readonly BotCommandHandlerManager _botCommandHandlerManager;
    private readonly IMessageSender _messageSender;

    public BotService(BotCommandHandlerManager botCommandHandlerManager, IMessageSender messageSender)
    {
        _botCommandHandlerManager = botCommandHandlerManager;
        _messageSender = messageSender;
    }

    public async Task ProcessCommand(BotCommandMessage botCommandMessage)
    {
        var commandResponse = await HandleCommand(botCommandMessage);

        _messageSender.Publish(new BotCommandResponse(commandResponse, botCommandMessage.UserName,
            botCommandMessage.GroupId));
    }

    private async Task<string> HandleCommand(BotCommandMessage botCommandMessage)
    {
        var commandSplit = botCommandMessage.Command.Split("=");

        if (commandSplit.Length != 2)
        {
            return "Invalid command";
        }

        var command = commandSplit[0];
        var value = commandSplit[1];

        if (string.IsNullOrWhiteSpace(value))
        {
            return "You must provide a stock symbol";
        }

        var handler = _botCommandHandlerManager.GetHandler(command);

        if (handler is null)
        {
            return "Command not found";
        }

        return await handler.HandleCommand(value);
    }
}