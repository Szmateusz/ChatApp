using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.SignalR;
using Blog.Hubs;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using System.Linq;

namespace Blog.Controllers
{
  //  [Authorize]
    public class ChatController : Controller
    {
        public readonly DBcontext _context;
        public readonly UserManager<UserModel> _userManager;


        public static int currentRoom=-1;
        public static bool firstStart=true;

       

        public ChatController(DBcontext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            
            string usrId = _userManager.GetUserId(User);
           
      
            
            if (firstStart == true)
            {
                try
                {
                    int firstRoom = _context.ConnectingToRooms.FirstOrDefault(u => u.UserSender.Id.Equals(usrId)).RoomId;

                    currentRoom = firstRoom;
                }
                catch
                {

                }
                

                firstStart =false;
            }

            if (currentRoom == -1)
            {
              return RedirectToAction("CreateRoom","Blog");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }

            
            
           IEnumerable<Room> roomsList =  _context.Rooms.ToList();

           var room = roomsList.FirstOrDefault(r=>r.Id==currentRoom);
           room.Messages = await _context.GroupMessages.Where(m=>m.RoomId==room.Id).ToListAsync();

            ViewData["currentRoom"] = currentRoom;

            IEnumerable<ConnectingToGroups> usersInGroupList = await _context.ConnectingToRooms.Where(x=>x.RoomId.Equals(currentRoom)).ToListAsync();

            IEnumerable<ConnectingToGroups> connectingGroups = await _context.ConnectingToRooms.Where(x=>x.UserSender.Id==currentUser.Id).ToListAsync();


            string? role = _context.ConnectingToRooms.FirstOrDefault(u => u.UserSender.Id.Equals(usrId)&& u.Roomsender.Equals(currentRoom)).Role;

            if (role!= null)
            {
                ViewData["role"] = role;

            }
            else
            {
                ViewData["role"] = "none";

            }
            IList<UserModel> usersList = await _context.Users.ToListAsync();

            foreach (var x in usersInGroupList)
            {
                usersList.Remove(x.UserSender);

            }

            ChatView model = new ChatView();

                model.Rooms = roomsList;
                model.Connecting = connectingGroups;
                model.Users = usersList;
                model.UsersInGroup = usersInGroupList;


            return View(model);

        }

        public async Task<IActionResult> Create(GroupMessage message)
        {
            
            if(message.Text!=String.Empty)
            {
                message.UserName = User.Identity.Name;
                
                message.RoomId = currentRoom;


                var sender = await _userManager.GetUserAsync(User);
                message.UserID = sender.Id;
                await _context.GroupMessages.AddAsync(message);
               
        
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

        [HttpGet]
        public IActionResult CreateRoom()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoom cr)
        {
            if (_context.Rooms.Any(r => r.Name == cr.Name))
            {
                ViewData["isCreated"]=true;
                return View();
            }
            var sender = await _userManager.GetUserAsync(User);

            var newRoom = new Room
            {   
                
                Name = cr.Name
            };

            _context.Rooms.Add(newRoom);
            _context.SaveChanges();

            int id = newRoom.Id;

            currentRoom = id;
            var newConnect = new ConnectingToGroups
            {
                Roomsender = newRoom,
                UserSender = sender

        };

            _context.ConnectingToRooms.Add(newConnect);
            _context.SaveChanges();

            return RedirectToAction("Index", "Blog");
        }
        
        public IActionResult Invite(string usrId)
        {
            if(_context.ConnectingToRooms.Any(c=>c.UserSender.Id==usrId && c.Roomsender.Id == currentRoom))
            {
                return RedirectToAction("Index", "Blog");

            }

            var usrSender = _context.Users.FirstOrDefault(x => x.Id.Equals(usrId));

            var roomSender = _context.Rooms.FirstOrDefault(x => x.Id.Equals(currentRoom));


            var newConnect = new ConnectingToGroups
            {
                UserSender = usrSender,
                Roomsender = roomSender
            };

            _context.ConnectingToRooms.Add(newConnect);
            _context.SaveChanges();

            string name = usrSender.UserName;
            return Json(new { success = true, name });

        }

    }
}
