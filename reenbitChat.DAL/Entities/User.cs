
using Microsoft.AspNetCore.Identity;

namespace reenbitChat.DAL.Entities;

public class User : IdentityUser<int>
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public string ProfilePhotoUrl { get; set; }
    public virtual ICollection<Message> MessagesSent { get; set; } = new List<Message>();
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
}
