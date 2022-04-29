namespace FinancialChat.Core.Notifications;

public record BotCommandReceivedNotification(string Command, string UserId, string GroupId) : NotificationBase;