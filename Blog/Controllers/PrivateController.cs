using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
[Authorize(AuthenticationSchemes = "Identity.Application")]
    public class PrivateController : Controller
    {
        public readonly DBcontext _context;
        public readonly UserManager<UserModel> _userManager;

        public static string ReceiverId = "";
        public static string usrId = "";
        public PrivateController(DBcontext context, UserManager<UserModel> userManager)
        { 
            _context = context;
            _userManager = userManager;
           

        }
        public IActionResult Index()
        {
           
           usrId = _userManager.GetUserId(User);



            var chats = _context.Users.ToList();

            ReceiverId = chats.FirstOrDefault().Id;

            

            
            

           BigViewPrivate model = new BigViewPrivate();
               model.Users = chats;
            // model.PrivateMessages = chats;

            ViewData["currentChat"]= ReceiverId;

            return View(model);
        }

        public IActionResult SelectGroup(string userId)
        {
            ReceiverId = userId;

            return RedirectToAction("Index", "Private");
        }

        public async Task<IActionResult> Create(PrivateMessage message)
        {

            if (message.Text != String.Empty)
            {
                message.GrantorId = usrId;

                message.RecipientId = ReceiverId;

                
                await _context.PrivateMessages.AddAsync(message);


                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }
    }
}
