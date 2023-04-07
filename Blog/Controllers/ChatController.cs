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
  [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class ChatController : Controller
    {
        public readonly DBcontext _context;
        public readonly UserManager<UserModel> _userManager;
        public readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public static int currentRoom=-1;
        public static bool firstStart=true;

       

        public ChatController(DBcontext context, UserManager<UserModel> userManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
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
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                

                firstStart =false;
            }

            if (currentRoom == -1)
            {
              return RedirectToAction("CreateRoom","Chat");
            }
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }

            
            
           Room room =  _context.Rooms.Include(m=>m.Messages).FirstOrDefault(r => r.Id.Equals(currentRoom));
            if (room == null)
            {
                return RedirectToAction("CreateRoom", "Chat");

            }
            List<GroupMessage> messages = room.Messages.ToList();

            

            ViewData["currentRoom"] = currentRoom;

            var usersInGroupList = await _context.ConnectingToRooms.Where(x=>x.RoomId.Equals(currentRoom)).Include(c=>c.UserSender).ToListAsync();
            var usersInGroupIds = usersInGroupList.Select(u => u.UserSender.Id).ToList();

            var usersList = await _context.Users
                .Where(u => !usersInGroupIds.Contains(u.Id))
                .ToListAsync();

            var connectingGroups = await _context.ConnectingToRooms.Where(x=>x.UserSender.Id==currentUser.Id).Include(c=>c.Roomsender).ToListAsync();

            
          

            

           


            ChatView model = new ChatView();

                model.GroupMessages = messages;
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
            // set group after leave from current group
            if(roomId == -1)
            {
                firstStart = true;
                return RedirectToAction("Index", "Chat");

            }
            currentRoom = roomId;

            return RedirectToAction("Index","Chat");
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoomView model, IFormFile file)
        {
            if (_context.Rooms.Any(r => r.Name == model.Room.Name))
            {
                model.ValidateData = "Pokój o tej nazwie już istnieje!";
                return View(model);
            }
            if (model.Room.Name.Length>12)
            {
                model.ValidateData = "Pokój ma zbyt długą nazwę!";
                return View(model);
            }
            if (model.Room.Name.Length < 3)
            {
                model.ValidateData = "Pokój ma zbyt krótką nazwę!";
                return View(model);
            }

            var sender = await _userManager.GetUserAsync(User);

            var newRoom = new Room
            {   
                
                Name = model.Room.Name
            };

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "lib/room_avatar", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Zapisz nazwę pliku w modelu
                newRoom.ImgUrl = fileName;
            }else { newRoom.ImgUrl = "default.png"; }

            _context.Rooms.Add(newRoom);
            _context.SaveChanges();

            int id = newRoom.Id;

            currentRoom = id;
            var newConnect = new ConnectingToRooms
            {
                Roomsender = newRoom,
                UserSender = sender,
                Role = "admin"

        };

            _context.ConnectingToRooms.Add(newConnect);
            _context.SaveChanges();

            return RedirectToAction("Index", "Chat");
        }
        
        public IActionResult Invite(string usrId)
        {
            if(_context.ConnectingToRooms.Any(c=>c.UserSender.Id==usrId && c.Roomsender.Id == currentRoom))
            {
                return RedirectToAction("Index", "Chat");

            }

            var usrSender = _context.Users.FirstOrDefault(x => x.Id.Equals(usrId));

            var roomSender = _context.Rooms.FirstOrDefault(x => x.Id.Equals(currentRoom));


            var newConnect = new ConnectingToRooms
            {
                UserSender = usrSender,
                Roomsender = roomSender
            };

            _context.ConnectingToRooms.Add(newConnect);
            _context.SaveChanges();

            string name = usrSender.UserName;
            string imgUrl = usrSender.ImgUrl; 

            return Json(new { success = true, name, imgUrl});

        }

    }
}
