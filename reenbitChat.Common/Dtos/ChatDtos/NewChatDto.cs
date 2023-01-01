using reenbitChat.Common.Dtos.UserDtos;

namespace reenbitChat.Common.Dtos.ChatDtos;

public class NewChatDto
{
    public string Name { get; set; }
    public IEnumerable<UserDto> Members { get; set; }
    public bool? IsGroup { get; set; }
}
