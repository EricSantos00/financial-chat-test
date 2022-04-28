using FinancialChat.Core.Features.Messages.Commands;
using FinancialChat.Core.Features.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Web.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private readonly IMediator _mediator;
    private static readonly IDictionary<string, string> _userCurrentRoom = new Dictionary<string, string>();

    private string Username => Context.User?.Identity?.Name ?? "Unknown";

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(string message)
    {
        if (!_userCurrentRoom.ContainsKey(Context.ConnectionId))
            throw new HubException("You are not in a room");

        await _mediator.Send(new PostMessageCommand(message, Username, _userCurrentRoom[Context.ConnectionId]));
    }

    public async Task JoinRoom(string room)
    {
        if (_userCurrentRoom.TryGetValue(Context.ConnectionId, out var oldRoom))
        {
            await LeaveRoom(oldRoom);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        _userCurrentRoom[Context.ConnectionId] = room;

        var messages = await _mediator.Send(new GetLatestRoomMessagesQuery(room));

        foreach (var message in messages)
        {
            await Clients.Caller.ReceiveMessage(message.UserId, message.Message);
        }
        
        await Clients.Group(room).ReceiveMessage(Username, $"{Username} joined the room");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_userCurrentRoom.TryGetValue(Context.ConnectionId, out var oldRoom))
        {
            await LeaveRoom(oldRoom);
        }
    }

    private async Task LeaveRoom(string room)
    {
        _userCurrentRoom.Remove(Context.ConnectionId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

        await Clients.Group(room).ReceiveMessage("System", $"{Username} left the room");
    }
}