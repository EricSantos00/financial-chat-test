using FinancialChat.Core.Entities;
using FinancialChat.Core.Interfaces;

namespace FinancialChat.Infrastructure.Data.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    public Task AddAsync(ChatMessage message)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<ChatMessage>> GetLatestMessagesAsync(string group, int count)
    {
        return null;
    }
}