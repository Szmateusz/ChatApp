using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class GroupMessage
    {

        public int Id { get; set; }

        public int RoomId { get; set; }

        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        
        public DateTime When { get; set; }

        public string UserID { get; set; }
        public virtual UserModel UserSender { get; set; }
        public virtual Room RoomSender { get; set; }




        public GroupMessage()
        {
            When = DateTime.Now;
        }
    }
}
