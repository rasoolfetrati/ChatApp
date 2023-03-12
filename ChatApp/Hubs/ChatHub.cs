using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChatApp.Hubs;
[Authorize]
public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
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
        var getUID = _context.UsersConnectionIds.Single(u => u.ConnectionId == receiverConnectionId).UserId;
        var getUName = _context.UsersConnectionIds.Include(u => u.User).Single(u => u.ConnectionId == receiverConnectionId).User.Email;
        if (ReciverIsOnline(receiverConnectionId))
        {
            await _context.Chats.AddAsync(new Chat()
            {
                UserId = int.Parse(Context.UserIdentifier!),
                SendDate = DateTime.Now,
                Text = messageServer,
                ReciverId = getUID
            });
            await _context.SaveChangesAsync();
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", messageServer, DateTime.Now.Date.ToString("dd/MM/yyyy"), getUName);

        }
        else
        {
            await _context.Chats.AddAsync(new Chat()
            {
                UserId = int.Parse(Context.UserIdentifier!),
                SendDate = DateTime.Now,
                Text = messageServer,
                ReciverId = getUID
            });
            await _context.SaveChangesAsync();
        }
    }

    public string GetUserId(string username)
    {
        var getUID = _context.Users.Single(u => u.Email == username).UserId;
        string userId = _context.UsersConnectionIds.OrderBy(o => o.ConsId).LastOrDefault(u => u.UserId == getUID).ConnectionId;
        return userId;
    }
    public string GetCurrentUserId()
    {
        string userId = _context.UsersConnectionIds.OrderBy(o => o.ConsId).LastOrDefault(u => u.UserId == int.Parse(Context.UserIdentifier)).ConnectionId;
        return userId;
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
