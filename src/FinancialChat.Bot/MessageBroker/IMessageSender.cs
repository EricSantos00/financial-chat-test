using FinancialChat.Bot.Messages;

namespace FinancialChat.Bot.MessageBroker;

public interface IMessageSender
{
    void Publish(MessageBase notification);
}