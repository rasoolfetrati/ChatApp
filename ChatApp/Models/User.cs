using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        [MaxLength(10)]
        public string UserStatus { get; set; }

        public ICollection<Chat> Chats { get; set; }
        public ICollection<UsersConnectionId> UsersConnectionIds { get; set; }
        public virtual ICollection<ConversationRoom> Rooms { get; set; }
    }
}
