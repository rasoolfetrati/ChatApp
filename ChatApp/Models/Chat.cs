using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class Chat
    {
        [Key] 
        public int ChatId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public int ReciverId { get; set; }
        [MaxLength(150)]
        public string Text { get; set; }
        public DateTime SendDate { get; set; }=DateTime.Now;

        public User User { get; set; }
    }
}
