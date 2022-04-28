using MediatR;

namespace FinancialChat.Core.Notifications;

public record MessagePostedNotification(string Message, string UserId, string GroupId) : INotification;