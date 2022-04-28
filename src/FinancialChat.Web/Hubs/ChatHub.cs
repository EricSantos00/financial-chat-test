using FinancialChat.Core.Features.Messages.Commands;
using FinancialChat.Core.Features.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Web.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private readonly IMediator _mediator;
    private readonly IDictionary<string, string> _userCurrentRoom = new Dictionary<string, string>();

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(string user, string message)
    {
        if (!_userCurrentRoom.ContainsKey(user))
            throw new HubException("You are not in a room");

        await _mediator.Send(new PostMessageCommand(message, user, _userCurrentRoom[Context.ConnectionId]));
    }

    public async Task JoinRoom()
    {
        // ...logic to join room
        
        var userRoom = _userCurrentRoom[Context.ConnectionId];
        
        var messages = await _mediator.Send(new GetLatestRoomMessagesQuery(userRoom));

        foreach (var message in messages)
        {
            await Clients.Caller.ReceiveMessage(message.UserId, message.Message);
        }
    }
}