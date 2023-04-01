

namespace Blog.Models
{
    public class ChatView
    {
        public IEnumerable<GroupMessage> GroupMessages  { get; set; }


        public IEnumerable<ConnectingToRooms> Connecting { get; set; }

        public IEnumerable<UserModel> Users  { get; set; }

        public IEnumerable<ConnectingToRooms> UsersInGroup { get; set; }



    }
}
