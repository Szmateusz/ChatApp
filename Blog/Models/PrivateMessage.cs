using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class PrivateMessage
    {
        
        public int Id { get; set; }

        public string GrantorId { get; set; }
        public string RecipientId { get; set; }
        public virtual UserModel Sender { get; set; }
        public virtual UserModel Receiver { get; set; }


        [Required]
        public string Text { get; set; }
        
        public DateTime When { get; set; }



        public PrivateMessage()
        {
            When = DateTime.Now;
        }
    }
}
