using Blog.Models;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message) =>
        await Clients.All.SendAsync("receiveMessage", message);

       public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public Task SendMessageToGroup(string group, Message message)
        {
            return Clients.Group(group).SendAsync("receiveMessage", message);
        }




    }
}
