using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class PrivateMessage
    {

        public int Id { get; set; }

        public int GrantorId { get; set; }
        public int RecipientId { get; set; }

        [Required]
        public string Text { get; set; }
        
        public DateTime When { get; set; }

        public virtual UserModel Grantor { get; set; }
        public virtual UserModel Recipient { get; set; }






        public Message()
        {
            When = DateTime.Now;
        }
    }
}
