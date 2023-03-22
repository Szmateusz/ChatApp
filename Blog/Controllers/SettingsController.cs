using Blog;
using Blog.Models;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class SettingsController : Controller
    {

        public readonly DBcontext _context;
        public readonly UserManager<UserModel> _userManager;



        public SettingsController(DBcontext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;



        }

        public IActionResult Index()
        {
          SettingsView model = new SettingsView();
            
          var userId = _userManager.GetUserId(User);

          var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));

          model.User = user;

          return View(model);
        }

        [HttpPost]
        public IActionResult ChangeUserData(SettingsView file)
        {
            var data = file;
            return RedirectToAction("Index");
        }
    }
}
