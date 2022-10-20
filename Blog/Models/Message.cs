using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Message
    {

        public int Id { get; set; }

        public int RoomId { get; set; }

        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        
        public DateTime When { get; set; }

        public string UserID { get; set; }
        public virtual UserModel Sender { get; set; }
        public virtual Room RoomSender { get; set; }




        public Message()
        {
            When = DateTime.Now;
        }
    }
}
