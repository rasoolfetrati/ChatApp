using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class ConversationRoom
    {
        [Key]
        public string RoomName { get; set; }
        [MaxLength(100)]
        public string User1 { get; set; }
        [MaxLength(100)]
        public string User2 { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
