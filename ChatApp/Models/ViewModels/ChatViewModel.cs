namespace ChatApp.Models.ViewModels;

public class ChatViewModel
{
    public int ChatId { get; set; }
    public string UserId { get; set; }
    public string ReciverId { get; set; }
    public string Text { get; set; }
    public string SendDate { get; set; }
    public bool IsCurrentUser { get; set; }
}
