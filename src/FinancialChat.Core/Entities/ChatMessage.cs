namespace FinancialChat.Core.Entities;

public record ChatMessage(Guid Id, string Message, string UserId, string GroupId, DateTime CreatedAt);