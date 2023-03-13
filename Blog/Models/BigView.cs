

namespace Blog.Models
{
    public class BigView
    {
        public IEnumerable<GroupMessage> GroupMessages  { get; set; }

        public IEnumerable<Room> Rooms{ get; set; }

        public IEnumerable<ConnectingToGroups> Connecting { get; set; }

        public IEnumerable<UserModel> Users  { get; set; }

        public IEnumerable<ConnectingToGroups> UsersInGroup { get; set; }



    }
}
