using FinancialChat.Core.Features.Messages.Commands;
using FinancialChat.Core.Features.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Web.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IMediator _mediator;
    private static readonly IDictionary<string, string> UserCurrentRoom = new Dictionary<string, string>();

    private string Username => Context.User?.Identity?.Name ?? "Unknown";

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(string message)
    {
        if (!UserCurrentRoom.ContainsKey(Context.ConnectionId))
            throw new HubException("You are not in a room");

        await _mediator.Send(new PostMessageCommand(message, Username, UserCurrentRoom[Context.ConnectionId]));
    }

    public async Task JoinRoom(string room)
    {
        if (UserCurrentRoom.TryGetValue(Context.ConnectionId, out var oldRoom))
        {
            if (oldRoom == room)
                throw new HubException("You are already in this room");

            await LeaveRoom(oldRoom);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        UserCurrentRoom[Context.ConnectionId] = room;

        var messages = await _mediator.Send(new GetLatestRoomMessagesQuery(room));

        await Clients.Caller.JoinedRoom();

        foreach (var message in messages)
        {
            await Clients.Caller.ReceiveMessage(message.CreatedAt, message.UserName, message.Message);
        }

        await Clients.Group(room).ReceiveMessage(DateTime.Now, "[System]", $"{Username} joined the room");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (UserCurrentRoom.TryGetValue(Context.ConnectionId, out var oldRoom))
        {
            await LeaveRoom(oldRoom);
        }
    }

    private async Task LeaveRoom(string room)
    {
        UserCurrentRoom.Remove(Context.ConnectionId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

        await Clients.Group(room).ReceiveMessage(DateTime.Now, "[System]", $"{Username} left the room");
    }
}