namespace FinancialChat.Bot.BotCommandHandler;

public interface IBotCommandHandler
{
    Task<string> HandleCommand(string commandValue);
}