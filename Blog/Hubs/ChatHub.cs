using Blog.Models;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message) =>
            await Clients.All.SendAsync("receiveMessage", message);

        public async Task SendMessageToGroup(string groupId, string message) =>
            await Clients.Group(groupId).SendAsync(message);

    }
}
