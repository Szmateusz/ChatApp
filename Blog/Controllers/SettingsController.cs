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
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public SettingsController(DBcontext context, UserManager<UserModel> userManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hosting;



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
        public async Task<IActionResult> ChangeUserData(SettingsView model, IFormFile file)
        {

            // Pobierz i zapisz plik na dysku
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath,"lib/user_avatar", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Zapisz nazwę pliku w modelu
                model.User.ImgUrl = fileName;
            }

            // Zapisz dane użytkownika w bazie danych
            string userId =  _userManager.GetUserId(User);
            var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));

            user.ImgUrl = model.User.ImgUrl;

            if(user.UserName!=model.User.UserName && model.User.UserName.Length> 5) { 
                user.UserName = model.User.UserName;
             }
            if (user.Email != model.User.Email)
            {
                user.Email = model.User.Email;
            }
            if (user.PhoneNumber != model.User.PhoneNumber && model.User.PhoneNumber.Length >= 9)
            {
                user.PhoneNumber = model.User.PhoneNumber;
            }

            _context.Users.Update(user);         
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Settings");


        }

        public IActionResult Groups()
        {

            return View();
        }

    }
}
