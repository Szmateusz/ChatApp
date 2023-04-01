using Blog.Models;

namespace ChatApp.Models
{
    public class SettingsIndexView
    {
        public UserModel User { get; set; }
       
        public string? Password { get; set; }
    }
}
