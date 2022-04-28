using FinancialChat.Core.Notifications;
using FinancialChat.Web.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Web.Handlers;

public class MessagePostedNotificationHandler : INotificationHandler<MessagePostedNotification>
{
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public MessagePostedNotificationHandler(IHubContext<ChatHub, IChatClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(MessagePostedNotification notification, CancellationToken cancellationToken)
    {
        var (message, userId, groupId) = notification;

        await _hubContext.Clients.Group(groupId).ReceiveMessage(userId, message);
    }
}