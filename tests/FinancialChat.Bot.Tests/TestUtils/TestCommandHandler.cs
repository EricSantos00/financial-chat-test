using System.Threading.Tasks;
using FinancialChat.Bot.BotCommandHandler;

namespace FinancialChat.Bot.Tests.TestUtils;

public class TestCommandHandler : IBotCommandHandler
{
    public Task<string> HandleCommand(string commandValue)
    {
        return Task.FromResult(commandValue);
    }
}