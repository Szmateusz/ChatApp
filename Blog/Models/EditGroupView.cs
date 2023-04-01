using Blog.Models;

namespace ChatApp.Models
{
    public class EditGroupView
    {
        public Room Room { get; set; }
        public List<ConnectingToRooms> Users { get; set; }
    }
}
