using Blog.Migrations;

namespace Blog.Models
{
    public class BigView
    {
        public IEnumerable<GroupMessage> Messages  { get; set; }

        public IEnumerable<Room> Rooms{ get; set; }

        public IEnumerable<ConnectingToGroups> Connecting { get; set; }

        public IEnumerable<UserModel> Users  { get; set; }

        public IEnumerable<ConnectingToGroups> UsersInGroup { get; set; }


    }
}
