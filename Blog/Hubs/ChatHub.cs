using Blog.Models;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Hubs
{
    public class ChatHub : Hub
    {
        public readonly DBcontext _context;

        public ChatHub(DBcontext context) { 
            _context = context;
            
        }
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

        public Task InviteToGroup(int group,string user)
        {
            
            //var room = _context.Rooms.FirstOrDefault(r => r.Id.Equals(group));
            var room = _context.Rooms.FirstOrDefault(r => r.Id.Equals(group));
            string groupName = room.Name;
            string groupId = room.Id.ToString();
            return Clients.User(user).SendAsync("receiveToGroup", groupName, groupId);
        }


    }
}
