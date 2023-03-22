using Blog.Models;

namespace ChatApp.Models
{
    public class SettingsView
    {
        public UserModel User { get; set; }
        public byte[]? File { get; set; }
        public string? Password { get; set; }
    }
}
