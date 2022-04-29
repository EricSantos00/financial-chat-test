using FinancialChat.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Infrastructure.Logger;

public class LoggerAdapter<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }
}