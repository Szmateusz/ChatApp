using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Register
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Pole  'Nazwa Użytkownika' jest wymagane")]
        [StringLength(30, ErrorMessage = "Pole 'Nazwa uzytkownika' może zawierać od 6 do 30 znaków.")]
        public string UserName { get; set; }


        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Pole 'hasło' jest wymagane.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Pole 'hasło' może zawierać od 6 do 30 znaków.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'Adres e-mail' musi być prawidłowym adresem e-mail.")]
        public string Email { get; set; }
    }
}
