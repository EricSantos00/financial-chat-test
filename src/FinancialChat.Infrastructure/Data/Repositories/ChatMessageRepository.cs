using FinancialChat.Core.Entities;
using FinancialChat.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infrastructure.Data.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ChatMessageRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task AddAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        await _applicationDbContext.AddAsync(message, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<ChatMessage>> GetLatestMessagesAsync(string @group, int count,
        CancellationToken cancellationToken)
    {
        return _applicationDbContext.ChatMessages
            .AsNoTracking()
            .Where(x => x.GroupId == group)
            .OrderBy(x => x.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}