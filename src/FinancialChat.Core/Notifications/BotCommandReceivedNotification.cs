namespace FinancialChat.Core.Notifications;

public record BotCommandReceivedNotification(string Command, string UserName, string GroupId) : NotificationBase;