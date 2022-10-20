using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class LogIn
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
