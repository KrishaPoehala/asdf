using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using reenbitChat.BLL.Hubs;
using reenbitChat.Common.Dtos.ChatDtos;
using reenbitChat.Common.Dtos.MessageDtos;
using reenbitChat.Common.Dtos.UserDtos;
using reenbitChat.DAL.Context;
using reenbitChat.DAL.Entities;
using reenbitChat.Domain.Services;

namespace reenbitChat.BLL.Services;

public class ChatsService : ServiceBase, IChatService
{
    public ChatsService(ChatContext context, IHubContext<ChatHub> hub, IMapper mapper)
        : base(context, hub, mapper)
    {
    }

    public async Task<ChatDto> CreateChat(NewChatDto newChatDto)
    {
        var members = newChatDto.Members;
        newChatDto.Members = null;
        var chat = _mapper.Map<Chat>(newChatDto);
        _context.Chats.Add(chat);
        foreach (var memberId in members.Select(x => x.Id))
        {
            var user = await _context.Users.FirstAsync(x => x.Id == memberId);
            user.Chats.Add(chat);
        }

        await _context.SaveChangesAsync();
        var result = _mapper.Map<ChatDto>(chat);
        result.Members = members;
        await _hub.Clients.All.SendAsync("ChatCreated", result);
        return result;
    }

    public async Task<IEnumerable<MessageDto>> GetChatMessages(int chatId, int userId, int pageNumber, int messagesInPage)
    {
        var chat = await _context.Chats
       .Include(x => x.Messages)
       .ThenInclude(x => x.Sender)
       .FirstAsync(x => x.Id == chatId);

        var messages = chat.Messages;
        var messagesToSkip = messagesInPage * pageNumber;
        if (messages.Count <= messagesToSkip)
        {
            return Enumerable.Empty<MessageDto>();
        }

        return messages
            .Where(x => x.SenderId != userId || x.IsDeletedOnlyForSender == false)
            .OrderBy(x => x.SentAt)
            .Skip(messagesToSkip)
            .Take(messagesInPage)
            .Select(x => _mapper.Map<MessageDto>(x));
    }

    public async Task<UserDto> GetRandomUser()
    {
        var max = await _context.Users.CountAsync();
        var rnd = Random.Shared.Next(1, max + 1);
        var user = await _context.Users.FirstAsync(x => x.Id == 5);
        //var user = await _context.Users.FirstAsync(x => x.Id == 5);
        //this method gets rnd user each time when the page is reloaded.
        //so if you'd like to test the method specify the id number
        return _mapper.Map<UserDto>(user);
    }

    public IEnumerable<ChatDto> GetUserChats(int userId)
    {
        return _context.Chats
        .Include(x => x.Members)
        .Where(x => x.Members.Any(x => x.Id == userId))
        .Select(x => _mapper.Map<ChatDto>(x));
    }
}
