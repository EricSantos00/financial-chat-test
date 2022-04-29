using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.Bot.BotMessageHandler;

public class BotMessageHandlerManager
{
    private readonly Dictionary<string, IBotMessageHandler?> _botMessageHandlers = new();
    private readonly IServiceProvider _serviceProvider;

    public BotMessageHandlerManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Register<T>(string command)
        where T : IBotMessageHandler
    {
        if (_botMessageHandlers.ContainsKey(command))
        {
            throw new Exception($"Command {command} is already registered");
        }
        
        _botMessageHandlers.Add(command, ActivatorUtilities.CreateInstance<T>(_serviceProvider));
    }

    public IBotMessageHandler? GetHandler(string command)
    {
        return !_botMessageHandlers.ContainsKey(command) ? null : _botMessageHandlers[command];
    }
}