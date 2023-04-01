using Blog.Models;

namespace ChatApp.Models
{
    public class SettingsGroupsView
    {

        public List<Room> rooms { get; set; }
        public List<ConnectingToRooms> Connectings { get; set; }
        public List<ConnectingToRooms> ConnectingsWhereAdmin { get; set; }


    }
}
