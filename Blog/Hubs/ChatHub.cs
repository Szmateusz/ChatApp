using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Hubs
{
    public class ChatHub : Hub
    {
        public readonly DBcontext _context;
        


        public ChatHub(DBcontext context) { 
            _context = context;
            

            
        }
        public async Task SendMessage(GroupMessage message) =>
        await Clients.All.SendAsync("receiveMessage", message);

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        public Task LeaveGroup(string group,string user)
        {
            
            string userId = _context.Users.FirstOrDefault(u => u.UserName.Equals(user)).Id;
            var ConnToDelete = _context.ConnectingToRooms.FirstOrDefault(r => r.Roomsender.Id == int.Parse(group) && r.UserSender.Id == userId);
           
            
            _context.ConnectingToRooms.Remove(ConnToDelete);
            _context.SaveChanges();

            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }

        public Task DeleteUserFromGroup(string user,string group)
        {
            return Clients.Group(group).SendAsync("deleteFromGroup", user);

        }

        public Task SendMessageToGroup(string group, GroupMessage message)
        {
            string img = _context.Users.FirstOrDefault(x => x.UserName == message.UserName).ImgUrl;
            return Clients.Group(group).SendAsync("receiveMessage", message, img);
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
