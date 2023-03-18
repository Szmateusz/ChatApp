using System.Collections;

namespace Blog.Models
{
    public class Room
    {
        public Room()
        {
            Messages = new HashSet<GroupMessage>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public virtual ICollection<GroupMessage> Messages { get; set; }
    }
}
