namespace Blog.Models
{
    public class ConnectingToGroups
    {
        public int Id { get; set; }
       
        public UserModel UserSender { get; set; }

        public Room Roomsender { get; set; }
    }
}
