namespace FinancialChat.Core.Entities;

public record ChatMessage(Guid Id, string Message, string UserName, string GroupId, DateTime CreatedAt);