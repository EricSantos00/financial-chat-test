using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.Bot.BotCommandHandler;

public class BotCommandHandlerManager
{
    private readonly Dictionary<string, IBotCommandHandler?> _botMessageHandlers = new();
    private readonly IServiceProvider _serviceProvider;

    public BotCommandHandlerManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Register<T>(string command)
        where T : IBotCommandHandler
    {
        if (_botMessageHandlers.ContainsKey(command))
        {
            throw new Exception($"Command {command} is already registered");
        }
        
        _botMessageHandlers.Add(command, ActivatorUtilities.CreateInstance<T>(_serviceProvider));
    }

    public IBotCommandHandler? GetHandler(string command)
    {
        return !_botMessageHandlers.ContainsKey(command) ? null : _botMessageHandlers[command];
    }
}