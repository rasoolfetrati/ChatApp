using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;

public class ChatHub : Hub
{
    public static List<User> users = new List<User>();
    public override Task OnConnectedAsync()
    {
        if (!UserExists("saeed@gmail.com"))
        {
            users.Add(new User { Email = "saeed@gmail.com", ConnectionId = GetConnectionId() });
        }
        return base.OnConnectedAsync();
    }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendToUser( string receiverConnectionId, string message)
    {
        await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", message);
        await Task.CompletedTask;
    }
    public string RegisterUser(string username)
    {
        if (!users.Any(u => u.Email == username))
        {
            var user2 = new User()
            {
                ConnectionId = GetConnectionId(),
                Email = "ali@gmail.com",
            };
            users.Add(user2);
            return user2.ConnectionId;
        }
        else
        {
            return GetUserId(username);
        }
    }
    public string GetUserId(string username)
    {
        var usern = users.ToList();
        var conId = users.Single(u => u.Email == username).ConnectionId;
        return conId;
    }
    static bool UserExists(string email)
    {
        foreach (var user in users)
        {
            if (user.Email == email)
            {
                return true;
            }
        }
        return false;
    }
    public string GetConnectionId() => Guid.NewGuid().ToString().Replace("-","");
}
