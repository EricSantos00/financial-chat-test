namespace FinancialChat.Core.Interfaces;

public interface IAppLogger<T>
{
    void LogInformation(string message, params object[] args);
}