namespace FinancialChat.Bot.MessageBroker;

public interface IMessageReceiver<out T>
{
    void Receive(Func<T, Task> action);
}