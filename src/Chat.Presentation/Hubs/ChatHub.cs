using Chat.BL.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Presentation.Hubs;

public sealed class ChatHub : Hub, IChatHub
{
    public const string ReceiveMessage = nameof(ReceiveMessage);

    private readonly Dictionary<string, List<string>> _groups = new();

    public async Task DisbandGroupAsync(string chatId)
    {
        if (_groups.TryGetValue(chatId, out var connectionIds))
        {
            foreach (var connectionId in connectionIds)
                await Groups.RemoveFromGroupAsync(connectionId, chatId);

            _groups.Remove(chatId);
        }
    }

    public async Task JoinChatAsync(string chatId)
    {
        if (!_groups.TryGetValue(chatId, out var connectionIds))
        {
            connectionIds = [];
            _groups[chatId] = connectionIds;
        }

        connectionIds.Add(Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChatAsync(string chatId)
    {
        if (_groups.TryGetValue(chatId, out var connectionIds))
            connectionIds.Remove(Context.ConnectionId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task SendMessageAsync(string chatId, Message message)
    {
        await Clients.Group(chatId).SendAsync(ReceiveMessage, message);
    }
}