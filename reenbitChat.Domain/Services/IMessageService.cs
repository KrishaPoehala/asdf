using reenbitChat.Common.Dtos.MessageDtos;

namespace reenbitChat.Domain.Services;


public interface IMessageService
{
    Task SendMessage(NewMessageDto message);
    Task EditMessage(EditMessageDto dto);
    Task DeleteMessage(int id, bool isDeleteOnlyForSender);
}
