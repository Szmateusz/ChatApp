using System.Collections;

namespace Blog.Models
{
    public class Room
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
