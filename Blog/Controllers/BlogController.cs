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


        public static int currentRoom=1;

       
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

            
            
            IEnumerable<Room> roomsList =  _context.Rooms.ToList();

            var room = roomsList.FirstOrDefault(r=>r.Id==currentRoom);
            room.Messages = await _context.Messages.Where(m=>m.RoomId==room.Id).ToListAsync();

            
            ViewData["currentRoom"] = currentRoom;

            IEnumerable<ConnectingToGroups> usersInGroupList = await _context.ConnectingToRooms.Where(x=>x.Roomsender.Id.Equals(currentRoom)).ToListAsync();
            IEnumerable<ConnectingToGroups> usersInGroupListUnique = usersInGroupList.DistinctBy(x => x.UserSender.UserName);

            IEnumerable<ConnectingToGroups> connectingGroups = await _context.ConnectingToRooms.Where(x=>x.UserSender.Id==currentUser.Id).ToListAsync();
            IEnumerable<ConnectingToGroups> connectingGroupsUnique = connectingGroups.DistinctBy(x => x.Roomsender.Name);

            IEnumerable<UserModel> usersList = await _context.Users.ToListAsync();
            



            BigView model = new BigView();


            model.Rooms = roomsList;
            model.Connecting = connectingGroupsUnique;
            model.Users = usersList;
            model.UsersInGroup = usersInGroupListUnique;


            return View(model);

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

        [HttpGet]
        public IActionResult CreateRoom()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoom cr)
        {
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
            var usrSender = _context.Users.FirstOrDefault(x => x.Id.Equals(usrId));

            var roomSender = _context.Rooms.FirstOrDefault(x => x.Id.Equals(currentRoom));


            var newConnect = new ConnectingToGroups
            {
                UserSender = usrSender,
                Roomsender = roomSender
            };

            _context.ConnectingToRooms.Add(newConnect);
            _context.SaveChanges();

            return RedirectToAction("Index", "Blog");
        }

    }
}
