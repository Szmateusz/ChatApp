using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blog.Models
{
    public class CreateRoomView
    {

       
       
        
        [Display(Name = "Nazwa grupy")]
        [Required(ErrorMessage = "Pole  'Nazwa Użytkownika' jest wymagane")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Pole 'Nazwa grupy' musi zawierać od 4 do 20 znaków.")]
        public string  Name { get; set; }     
        
    }
}
