using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.SignalR;
using Blog.Hubs;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Blog.Controllers
{
    //[Authorize]
    public class BlogController : Controller
    {
        public readonly DBcontext _context;
        public readonly UserManager<UserModel> _userManager;


        public static int currentRoom=6;

       
        public BlogController(DBcontext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }

        public async Task<IActionResult> Index()
        {
            
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
           
           // var messages = await _context.Messages.ToListAsync();
            IEnumerable<Message> messages = _context.Messages.Where(r => r.RoomId.Equals(currentRoom));

            IList<Room> roomsList  = await _context.Rooms.ToListAsync();

            
            
            ViewData["rooms"] = roomsList;
            ViewData["currentRoom"] = currentRoom;

            
            return View(messages);
        }

        public async Task<IActionResult> Create(Message message)
        {
            
            if(message.Text!=String.Empty)
            {
                message.UserName = User.Identity.Name;
                
                message.RoomId = currentRoom;


                var sender = await _userManager.GetUserAsync(User);
                message.UserID = sender.Id;
                await _context.Messages.AddAsync(message);
               
        
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }

        
        public IActionResult SelectGroup(int roomId)
        {
            currentRoom = roomId;

            return RedirectToAction("Index","Blog");
        }
    }
}
