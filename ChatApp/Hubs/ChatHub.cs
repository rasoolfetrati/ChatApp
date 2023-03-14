using ChatApp.Models;
using ChatApp.Models.Services;
using ChatApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

namespace ChatApp.Hubs;
[Authorize]
public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;
    public ChatHub(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    #region Override functions
    public override Task OnConnectedAsync()
    {
        _context.UsersConnectionIds.Add(new UsersConnectionId()
        {
            UserId = int.Parse(Context.UserIdentifier),
            ConnectionId = Context.ConnectionId,
        });
        var obj = _context.Users.Find(int.Parse(Context.UserIdentifier));
        obj.UserStatus = "Online";
        _context.Users.Update(obj);
        _context.SaveChanges();
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var obj = _context.Users.Find(int.Parse(Context.UserIdentifier));
        obj.UserStatus = "Offline";
        _context.Users.Update(obj);
        _context.SaveChanges();
        return base.OnDisconnectedAsync(exception);
    }
    #endregion

    //public async Task SendMessage(string user, string message)
    //{
    //    await Clients.All.SendAsync("ReceiveMessage", user, message);
    //}

    public async Task SendToUser(string receiverConnectionId, string messageServer)
    {
        var getUID = _context.UsersConnectionIds.FirstOrDefault(u => u.ConnectionId == receiverConnectionId).UserId;
        var getUName = _context.UsersConnectionIds.Include(u => u.User).FirstOrDefault(u => u.ConnectionId == receiverConnectionId).User.Email;
        var getCUName = _userService.GetnameByReciverId(int.Parse(Context.UserIdentifier));
        var getCurrentUserConnection = _userService.GetUserConnectionById(int.Parse(Context.UserIdentifier));
        var currentUser = _userService.GetnameByReciverId(int.Parse(Context.UserIdentifier!));
        var getRoom = _context.ConversationRooms.FirstOrDefault(c =>
                       (c.User1 == currentUser && c.User2 == getUName) ||
                       (c.User1 == getUName && c.User2 == currentUser));
        if (getRoom == null)
        {
            var createRoom = new ConversationRoom()
            {
                RoomName = Guid.NewGuid().ToString().Replace("-", ""),
                User1 = getCUName,
                User2 = getUName
            };
            await _context.ConversationRooms.AddAsync(createRoom);
            await _context.SaveChangesAsync();
        }
        if (ReciverIsOnline(receiverConnectionId))
        {
            await _context.Chats.AddAsync(new Chat()
            {
                UserId = int.Parse(Context.UserIdentifier!),
                SendDate = DateTime.Now,
                Text = messageServer,
                ReciverId = getUID,
            });
            await _context.SaveChangesAsync();
            string state = GetUserStatus(getUName);
            //await Groups.AddToGroupAsync(getCurrentUserConnection, getRoom.RoomName);
            //await Clients.Groups(getRoom.RoomName, getCurrentUserConnection, receiverConnectionId).SendAsync("ReceiveMessage", messageServer, DateTime.Now.Date.ToString("dd/MM/yyyy"), getUName, state);
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", messageServer, DateTime.Now.Date.ToString("dd/MM/yyyy"), getUName, state);
        }
        else
        {
            await _context.Chats.AddAsync(new Chat()
            {
                UserId = int.Parse(Context.UserIdentifier!),
                SendDate = DateTime.Now,
                Text = messageServer,
                ReciverId = getUID,
            });
            await _context.SaveChangesAsync();
        }
        await Task.CompletedTask;
    }

    public string GetUserId(string username)
    {
        var getUID = _context.Users.Single(u => u.Email == username).UserId;
        string userId = _context.UsersConnectionIds.OrderBy(o => o.ConsId).LastOrDefault(u => u.UserId == getUID).ConnectionId;
        if (userId != null)
        {
            return userId;
        }
        return String.Empty;
    }
    public string GetCurrentUserId()
    {
        string userId = _context.UsersConnectionIds.OrderBy(o => o.ConsId).LastOrDefault(u => u.UserId == int.Parse(Context.UserIdentifier)).ConnectionId;
        return userId;
    }
    public string GetUserStatus(string username)
    {
        string userstate = _context.Users.SingleOrDefault(u => u.Email == username).UserStatus;
        return userstate;
    }
    public List<ChatViewModel> GetUserMessages(string id)
    {
        var currentUserConnection = _userService.GetUserConnectionById(int.Parse(Context.UserIdentifier!));
        var currentUser = _userService.GetnameByReciverId(int.Parse(Context.UserIdentifier!));
        var getRoom = _context.ConversationRooms.FirstOrDefault(c =>
                       (c.User1 == currentUser && c.User2 == id) ||
                       (c.User1 == id && c.User2 == currentUser));
        Groups.AddToGroupAsync(currentUserConnection, getRoom.RoomName);
        List<ChatViewModel> chatViewModels = new List<ChatViewModel>();
        var currentUserId = int.Parse(Context.UserIdentifier!);
        var otherUserId = _context.Users.Single(u => u.Email == id).UserId;
        var messages = _context.Chats
            .Where(c =>
                (c.UserId == currentUserId && c.ReciverId == otherUserId) ||
                (c.UserId == otherUserId && c.ReciverId == currentUserId))
            .OrderBy(c => c.SendDate)
            .ToList();

        if (messages != null)
        {
            foreach (var message in messages)
            {
                bool isCurrentUser = message.UserId == currentUserId;
                chatViewModels.Add(new ChatViewModel()
                {
                    ChatId = message.ChatId,
                    Text = message.Text,
                    SendDate = message.SendDate.Date.ToString("dd/MM/yyyy"),
                    ReciverId = _userService.GetnameByReciverId(message.ReciverId),
                    UserId = isCurrentUser ? "You" : _userService.GetnameByReciverId(message.UserId),
                    IsCurrentUser = isCurrentUser,
                });
            }
        }

        return chatViewModels;
    }

    public bool ReciverIsOnline(string id)
    {
        string getUStatus = _context.UsersConnectionIds.Include(u => u.User).Single(u => u.ConnectionId == id).User.UserStatus;
        if (getUStatus == "Online")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
