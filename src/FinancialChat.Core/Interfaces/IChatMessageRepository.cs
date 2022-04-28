using FinancialChat.Core.Entities;

namespace FinancialChat.Core.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message, CancellationToken cancellationToken);
    Task<List<ChatMessage>> GetLatestMessagesAsync(string @group, int count, CancellationToken cancellationToken);
}