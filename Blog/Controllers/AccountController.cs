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

        public AccountController(UserManager<UserModel> usermanager, SignInManager<UserModel> signInManager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Blog");
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData)
        {
            if(!ModelState.IsValid)
            {
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