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

            TempData["ErrorMessage"] = "Podaj poprawne dane logowania";
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
        
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData)
        {
          

            if (!ModelState.IsValid)
            {
                return View(userRegisterData);
            }
            if (_context.Users.Any(x => x.UserName.Equals(userRegisterData.UserName))){

                TempData["ErrorMessage"] = "Taki użytkownik już istnieje";
                return View(userRegisterData);
            }
         
            var newUser = new UserModel
            {
                UserName = userRegisterData.UserName,
                Email = userRegisterData.Email,
                ImgUrl = "default.png"

            };

           await _usermanager.CreateAsync(newUser,userRegisterData.Password);

            TempData["SuccessMessage"] = "Konto zostało utworzone pomyślnie.";

            return RedirectToAction("Login", "Account");
        }
    }
}