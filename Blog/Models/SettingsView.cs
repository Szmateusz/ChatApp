using Blog.Models;

namespace ChatApp.Models
{
    public class SettingsView
    {
        public UserModel User { get; set; }
       
        public string? Password { get; set; }
    }
}
