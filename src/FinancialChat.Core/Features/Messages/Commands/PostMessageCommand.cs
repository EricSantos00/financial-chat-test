using FinancialChat.Core.Entities;
using FinancialChat.Core.Interfaces;
using FinancialChat.Core.Notifications;
using MediatR;

namespace FinancialChat.Core.Features.Messages.Commands;

public record PostMessageCommand(string Message, string UserId, string GroupId) : IRequest;

public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IMediator _mediator;

    public PostMessageCommandHandler(IChatMessageRepository chatMessageRepository, IMediator mediator)
    {
        _chatMessageRepository = chatMessageRepository;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(PostMessageCommand request, CancellationToken cancellationToken)
    {
        var (message, userId, groupId) = request;

        if (message.StartsWith("/"))
        {
            await _mediator.Publish(new BotCommandReceivedNotification(message, userId, groupId), cancellationToken);
        }
        else
        {
            await _chatMessageRepository.AddAsync(new ChatMessage(message, userId, groupId, DateTime.Now));
            await _mediator.Publish(new MessagePostedNotification(message, userId, groupId), cancellationToken);
        }

        return Unit.Value;
    }
}