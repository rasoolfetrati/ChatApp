using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class UsersConnectionId
    {
        [Key]
        public int ConsId { get; set; }
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
