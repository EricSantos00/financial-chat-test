using FinancialChat.Core.Entities;
using FinancialChat.Core.Interfaces;
using MediatR;

namespace FinancialChat.Core.Features.Messages.Queries;

public record GetLatestRoomMessagesQuery(string Room) : IRequest<List<ChatMessage>>;

public class GetLatestRoomMessagesQueryHandler : IRequestHandler<GetLatestRoomMessagesQuery, List<ChatMessage>>
{
    private readonly IChatMessageRepository _chatMessageRepository;

    private const int MaxLatestMessages = 50;

    public GetLatestRoomMessagesQueryHandler(IChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }

    public Task<List<ChatMessage>> Handle(GetLatestRoomMessagesQuery request,
        CancellationToken cancellationToken) =>
        _chatMessageRepository.GetLatestMessagesAsync(request.Room, MaxLatestMessages, cancellationToken);
}