namespace FinancialChat.Bot.BotMessageHandler;

public interface IBotMessageHandler
{
    Task<string> HandleMessage(string messageValue);
}