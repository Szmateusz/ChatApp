using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class UserModel : IdentityUser
    {
        public UserModel()
        {
            Messages = new HashSet<GroupMessage>();
        }
        public virtual ICollection<GroupMessage> Messages { get; set; }
       
    }
}
 