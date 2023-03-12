namespace ChatApp.Models.Services
{
    public interface IUserService
    {
        string GetnameByReciverId(int id);
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
    }
}
