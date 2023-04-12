using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Hubs;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class ChatController : Controller
    {
        private readonly DBcontext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private static int _currentRoom = -1;
        private static bool _firstStart = true;

        public ChatController(DBcontext context, UserManager<UserModel> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);

            if (_firstStart)
            {
                try
                {
                    int firstRoom = _context.ConnectingToRooms.FirstOrDefault(u => u.UserSender.Id.Equals(userId))?.RoomId ?? -1;
                    _currentRoom = firstRoom;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                _firstStart = false;
            }

            if (_currentRoom == -1)
            {
                return RedirectToAction(nameof(CreateRoom));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }

            Room room = await _context.Rooms.Include(m => m.Messages).FirstOrDefaultAsync(r => r.Id == _currentRoom);
            if (room == null)
            {
                return RedirectToAction(nameof(CreateRoom));
            }

            List<GroupMessage> messages = room.Messages.ToList();
            ViewData["currentRoom"] = _currentRoom;

            var usersInGroupList = await _context.ConnectingToRooms
                .Where(x => x.RoomId == _currentRoom)
                .Include(c => c.UserSender)
                .ToListAsync();
            var usersInGroupIds = usersInGroupList.Select(u => u.UserSender.Id).ToList();

            var usersList = await _context.Users
                .Where(u => !usersInGroupIds.Contains(u.Id))
                .ToListAsync();

            var connectingGroups = await _context.ConnectingToRooms
                .Where(x => x.UserSender.Id == currentUser.Id)
                .Include(c => c.Roomsender)
                .ToListAsync();

            var model = new ChatView
            {
                GroupMessages = messages,
                Connecting = connectingGroups,
                Users = usersList,
                UsersInGroup = usersInGroupList
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                return Ok();
            }

            message.UserName = User.Identity.Name;
            message.RoomId = _currentRoom;

            var sender = await _userManager.GetUserAsync(User);
            message.UserID = sender.Id;
            await _context.GroupMessages.AddAsync(message);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public IActionResult SelectGroup(int roomId)
        {
            // set group after leave from current group
            if (roomId == -1)
            {
                _firstStart = true;
                return RedirectToAction(nameof(Index));
            }

            _currentRoom = roomId;
            return RedirectToAction(nameof(Index));
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

            _currentRoom = id;
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
            if(_context.ConnectingToRooms.Any(c=>c.UserSender.Id==usrId && c.Roomsender.Id == _currentRoom))
            {
                return RedirectToAction("Index", "Chat");

            }

            var usrSender = _context.Users.FirstOrDefault(x => x.Id.Equals(usrId));

            var roomSender = _context.Rooms.FirstOrDefault(x => x.Id.Equals(_currentRoom));


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
