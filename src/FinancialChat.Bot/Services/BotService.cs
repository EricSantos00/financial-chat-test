using FinancialChat.Bot.BotMessageHandler;
using FinancialChat.Bot.MessageBroker;
using FinancialChat.Bot.Messages;

namespace FinancialChat.Bot.Services;

public class BotService
{
    private readonly BotMessageHandlerManager _botMessageHandlerManager;
    private readonly IMessageSender _messageSender;

    public BotService(BotMessageHandlerManager botMessageHandlerManager, IMessageSender messageSender)
    {
        _botMessageHandlerManager = botMessageHandlerManager;
        _messageSender = messageSender;
    }

    public async Task ProcessCommand(BotCommandMessage botCommandMessage)
    {
        var commandResponse = await HandleCommand(botCommandMessage);

        _messageSender.Publish(new BotCommandResponse(commandResponse, botCommandMessage.UserId,
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

        var handler = _botMessageHandlerManager.GetHandler(command);

        if (handler is null)
        {
            return "Command not found";
        }

        return await handler.HandleMessage(value);
    }
}