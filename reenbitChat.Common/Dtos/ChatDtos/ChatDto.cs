using reenbitChat.Common.Dtos.MessageDtos;
using reenbitChat.Common.Dtos.UserDtos;

namespace reenbitChat.Common.Dtos.ChatDtos;

public class ChatDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<MessageDto> Messages { get; set; }
    public IEnumerable<UserDto> Members { get; set; }
    public bool? IsGroup { get; set; }
    public string ImageUrl { get; set; }
}
