namespace FinancialChat.Core.Interfaces;

public interface IMessageReceiver<out T>
{
    void Receive(Func<T, Task> action);
}