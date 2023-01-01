using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using reenbitChat.BLL.Hubs;
using reenbitChat.Common.Dtos.MessageDtos;
using reenbitChat.DAL.Context;
using reenbitChat.DAL.Entities;
using reenbitChat.Domain.Services;

namespace reenbitChat.BLL.Services;

public class MessageService : ServiceBase, IMessageService
{
    public MessageService(ChatContext context, IHubContext<ChatHub> hub, IMapper mapper) : base(context, hub, mapper)
    {
    }

    public async Task DeleteMessage(int id, bool isDeleteOnlyForSender)
    {
        var messageToDelete = await _context.Messages
            .FirstAsync(x => x.Id == id);
        if (isDeleteOnlyForSender)
        {
            messageToDelete.IsDeletedOnlyForSender = true;
        }
        else
        {
            _context.Messages.Remove(messageToDelete);
        }

        await _context.SaveChangesAsync();
        if (isDeleteOnlyForSender == false)
        {
            await _hub.Clients
                .Group(messageToDelete.ChatId.ToString())
                .SendAsync("MessageDeleted", _mapper.Map<MessageDto>(messageToDelete));
        }
    }

    public async Task EditMessage(EditMessageDto dto)
    {
        var messageToEdit = await _context.Messages
           .FirstAsync(x => x.Id == dto.MessageId);

        messageToEdit.Text = dto.EditedText;
        await _context.SaveChangesAsync();
        await _hub.Clients
            .Group(messageToEdit.ChatId.ToString())
            .SendAsync("MessageEdited", _mapper.Map<MessageDto>(messageToEdit));
    }

    public async Task SendMessage(NewMessageDto dto)
    {
        var sender = dto.Sender;
        var message = _mapper.Map<Message>(dto);
        message.Sender = null;//DbUpdateException otherwise, I don't know why tho(
        await _context.Messages.AddAsync(message);

        await _context.SaveChangesAsync();
        var messageDto = _mapper.Map<MessageDto>(message);
        messageDto.Sender = sender;
        await _hub.Clients
            .Group(message.ChatId.ToString())
            .SendAsync("MessageSent", messageDto);
    }
}
