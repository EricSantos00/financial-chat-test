using FinancialChat.Core.Entities;
using FinancialChat.Core.Interfaces;
using FinancialChat.Core.Notifications;
using MediatR;

namespace FinancialChat.Core.Features.Messages.Commands;

public record PostMessageCommand(string Message, string UserName, string GroupId) : IRequest;

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
        var (message, userName, groupId) = request;

        if (message.StartsWith("/"))
        {
            await _mediator.Publish(new BotCommandReceivedNotification(message, userName, groupId), cancellationToken);
        }
        else
        {
            var createdAt = DateTime.Now;

            await _chatMessageRepository.AddAsync(
                new ChatMessage(Guid.NewGuid(), message, userName, groupId, createdAt), cancellationToken);

            await _mediator.Publish(new MessagePostedNotification(message, userName, groupId, createdAt),
                cancellationToken);
        }

        return Unit.Value;
    }
}