using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models.Services
{
    public interface IUserService
    {
        string GetnameByReciverId(int id);
        string GetUserConnectionById(int id);
        string GetUserConnectionByEmail(string email);
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public string GetnameByReciverId(int id)
        {
            string name = _context.Users.Single(u => u.UserId == id).Email;
            return name;
        }

        public string GetUserConnectionByEmail(string email)
        {
            string connection = _context.UsersConnectionIds.Include(u=>u.User).OrderBy(u => u.ConsId).LastOrDefault(c => c.User.Email == email).ConnectionId;
            return connection;
        }

        public string GetUserConnectionById(int id)
        {
            string connection = _context.UsersConnectionIds.OrderBy(u=>u.ConsId).LastOrDefault(c=>c.UserId==id).ConnectionId;
            return connection;
        }
    }
}
