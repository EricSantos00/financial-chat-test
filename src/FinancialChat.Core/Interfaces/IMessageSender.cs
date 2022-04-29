using FinancialChat.Core.Notifications;

namespace FinancialChat.Core.Interfaces;

public interface IMessageSender
{
    void Publish(NotificationBase notification);
}