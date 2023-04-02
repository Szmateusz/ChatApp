using Blog;
using Blog.Hubs;
using Blog.Models;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



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
        public async Task<IActionResult> Index(SettingsIndexView model, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));

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
            } else { model.User.ImgUrl = user.ImgUrl; }



           

            // if User dont make changes

            if (model.User.ImgUrl == null &&
                model.User.ImgUrl == user.ImgUrl &&
               user.UserName == model.User.UserName &&
               user.Email == model.User.Email &&
               user.PhoneNumber == model.User.PhoneNumber &&
               (model.Password == null ||
               !new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.PasswordHash, model.Password)
               .Equals(PasswordVerificationResult.Success)))
            {
                
                model.ValidateData = "Nie wprowadzono żadnych zmian!";
                return View(model);
            }

                

            if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.NewPassword))
            {

                string password = model.Password;
                string userhash = user.PasswordHash;

                var passwordHasher = new PasswordHasher<UserModel>();

                if (!new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.PasswordHash, model.Password)
                     .Equals(PasswordVerificationResult.Success))
                {
                    model.ValidateData = "Stare hasło jest niepoprawne!";
                    return View(model);
                }

            }

            if (user.ImgUrl != model.User.ImgUrl && model.User.ImgUrl!=null)
            {
                user.ImgUrl = model.User.ImgUrl;
            }
            if (user.UserName!=model.User.UserName && model.User.UserName.Length> 5) { 
                user.UserName = model.User.UserName;
             }
            if (user.Email != model.User.Email && model.User.Email != null)
            {
                user.Email = model.User.Email;
            }
            if (user.PhoneNumber != model.User.PhoneNumber && model.User.PhoneNumber.Length >= 9)
            {
                user.PhoneNumber = model.User.PhoneNumber;
            }


            _context.Users.Update(user);         
            _context.SaveChanges();
            
            return View(model);


        }

        public IActionResult Groups()
        {
            SettingsGroupsView model = new SettingsGroupsView();

            string userId = _userManager.GetUserId(User);

            var userRooms = _context.ConnectingToRooms.Where(x => x.UserSender.Id.Equals(userId)).Include(c => c.Roomsender).Include(b=>b.UserSender).ToList();
          
            var adminRooms = userRooms.Where(x => x.Role == "admin").ToList();

            model.Connectings = userRooms;
            model.ConnectingsWhereAdmin = adminRooms;

            return View(model);
        }

        public IActionResult EditGroup(int id)
        {
            int _id = id;

            string usrId = _userManager.GetUserId(User);

            var room = _context.Rooms.FirstOrDefault(x => x.Id==_id);
            var users = _context.ConnectingToRooms.Where(x => x.Roomsender.Id.Equals(room.Id) && x.UserSender.Id!=usrId).Include(c=>c.UserSender).ToList();

            EditGroupView model = new EditGroupView();
            model.Room = room;
            model.Users = users;

            return View(model);
        }
        public async Task<IActionResult> DeleteUser(string userId, int groupId)
        {
           var connecting = await _context.ConnectingToRooms
                .FirstOrDefaultAsync(x => x.UserSender.Id.Equals(userId)
                && x.Roomsender.Id==groupId);

            if (connecting == null) { return NotFound(); }

            _context.ConnectingToRooms.Remove(connecting);
            await _context.SaveChangesAsync();
            int id = groupId;
            return RedirectToAction("EditGroup","Settings", new { id = id });
        }
     


    }
}
