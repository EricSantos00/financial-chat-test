namespace FinancialChat.Core.Notifications;

public record MessagePostedNotification(string Message, string UserName, string GroupId, DateTime CreatedAt) : NotificationBase;