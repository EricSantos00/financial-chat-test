namespace FinancialChat.Core.Entities;

public record ChatMessage(string Message, string UserId, string GroupId, DateTime CreatedAt);