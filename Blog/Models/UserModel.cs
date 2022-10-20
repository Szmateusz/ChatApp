using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class UserModel : IdentityUser
    {
        public UserModel()
        {
            Messages = new HashSet<Message>();
        }
        public virtual ICollection<Message> Messages { get; set; }
       
    }
}
 