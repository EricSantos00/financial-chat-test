using FinancialChat.Core.Entities;

namespace FinancialChat.Core.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message);
    Task<IEnumerable<ChatMessage>> GetLatestMessagesAsync(string group, int count);
}