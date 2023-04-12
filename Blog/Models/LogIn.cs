using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class LogIn
    {
        [Display(Name = "Login")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Hasło")]
        [Required]
        public string Password { get; set; }
    }
}
