using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UsersConnectionId> UsersConnectionIds { get; set; }
        public DbSet<ConversationRoom> ConversationRooms { get; set; }
    }
}
