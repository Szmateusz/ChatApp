using Blog;
using Blog.Models;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
          SettingsIndexView model = new SettingsIndexView();
            
          var userId = _userManager.GetUserId(User);

          var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));

          model.User = user;

          return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserData(SettingsIndexView model, IFormFile file)
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
            SettingsGroupsView model = new SettingsGroupsView();

            string userId = _userManager.GetUserId(User);

            var belongRooms = _context.ConnectingToRooms.Where(x => x.UserSender.Id.Equals(userId)).Include(c => c.Roomsender).Include(b=>b.UserSender).ToList();
          
            var adminRooms = belongRooms.Where(x => x.Role == "admin").ToList();

            model.Connectings = belongRooms;
            model.ConnectingsWhereAdmin = adminRooms;

            return View(model);
        }

        public IActionResult EditGroup([FromQuery] int id)
        {
            int _id = id;

            var room = _context.Rooms.FirstOrDefault(x => x.Id==_id);
            var users = _context.ConnectingToRooms.Where(x => x.Roomsender.Id.Equals(room.Id)).Include(c=>c.UserSender).ToList();

            EditGroupView model = new EditGroupView();
            model.Room = room;
            model.Users = users;

            return View(model);
        }
        public async Task<IActionResult> DeleteUser(string userId, int groupId)
        {
           var connecting = await _context.ConnectingToRooms.FirstOrDefaultAsync(x => x.UserSender.Id.Equals(userId) && x.Roomsender.Id==groupId);

            _context.ConnectingToRooms.Remove(connecting);
            await _context.SaveChangesAsync();
            int id = groupId;
            return RedirectToAction("EditGroup","Settings",id);
        }
    }
}
