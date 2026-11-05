using Lab6.Data;
using Lab6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Lab6.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string user, string message)
    {
        var msg = new Message
        {
            SenderName = user,
            MessageBody = message,
            MessageType = 0,
            CreatedAt = DateTime.Now
        };

        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinRoom(string roomName, string user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        await Clients.Group(roomName)
            .SendAsync("ReceiveMessage", "System", $"{user} joined room {roomName}");
    }

    public async Task LeaveRoom(string roomName, string user)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

        await Clients.Group(roomName)
            .SendAsync("ReceiveMessage", "System", $"{user} left room {roomName}");
    }

    public async Task SendGroupMessage(string roomName, string user, string message)
    {
        var msg = new Message
        {
            SenderName = user,
            RoomName = roomName,
            MessageBody = message,
            MessageType = 1,
            CreatedAt = DateTime.Now
        };

        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        await Clients.Group(roomName)
            .SendAsync("ReceiveMessage", user, $"[Room: {roomName}] {message}");
    }

    public async Task SendPrivateMessage(string receiverConnectionId, string user, string message)
    {
        var msg = new Message
        {
            SenderName = user,
            MessageBody = message,
            MessageType = 2,
            CreatedAt = DateTime.Now
        };

        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        await Clients.Client(receiverConnectionId)
            .SendAsync("ReceiveMessage", user, $"[Private] {message}");

        await Clients.Caller
            .SendAsync("ReceiveMessage", user, $"[Private to {receiverConnectionId}] {message}");
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
}