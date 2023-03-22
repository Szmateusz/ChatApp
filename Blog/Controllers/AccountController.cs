using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<UserModel> _usermanager;
        private readonly SignInManager<UserModel> _signInManager;

        public readonly DBcontext _context;

        public AccountController(DBcontext context,UserManager<UserModel> usermanager, SignInManager<UserModel> signInManager)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Chat");
            }
            
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LogIn userLogInData)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogInData);
            }

            await _signInManager.PasswordSignInAsync(userLogInData.UserName, userLogInData.Password, false, false);

            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["isRegistered"] = false;
            ViewData["isPasswordLenght"] = true;


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData)
        {
            ViewData["isPasswordLenght"] = true;
            ViewData["isRegistered"] = false;


            if (!ModelState.IsValid)
            {
                return View(userRegisterData);
            }
            if (_context.Users.Any(x => x.UserName.Equals(userRegisterData.UserName))){
                ViewData["isRegistered"] = true;
                
                return View(userRegisterData);
            }
            if (userRegisterData.Password.Length < 5)
            {
                ViewData["isPasswordLenght"] = false;
                return View(userRegisterData);

            }




            var newUser = new UserModel
            {
                UserName = userRegisterData.UserName
            };

           await _usermanager.CreateAsync(newUser,userRegisterData.Password);



            return RedirectToAction("Index", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}