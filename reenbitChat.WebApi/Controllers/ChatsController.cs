using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reenbitChat.Common.Dtos.ChatDtos;
using reenbitChat.Common.Dtos.MessageDtos;
using reenbitChat.Common.Dtos.UserDtos;
using reenbitChat.Domain.Services;

namespace reenbitChat.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;
    public ChatsController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    [Route("chats/{userId}")]
    public IActionResult GetChats(int userId)
    {
        return Ok(_chatService.GetUserChats(userId));
    }

    [HttpGet]
    [Route("messages/{chatId}/{userId}/{pageNumber}/{messagesToLoad}")]
    public async Task<IActionResult> GetMessages(int chatId, int userId,
int pageNumber = 0, int messagesToLoad = 20)
    {
        return Ok(await _chatService.GetChatMessages(chatId, userId,
            pageNumber, messagesToLoad));
    }

    [HttpGet]
    [Route("messages/{chatId}")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        return Ok(await _chatService.GetChatMessages(chatId, -1, 0, 20));
    }

    [HttpGet]
    [Route("randomUser")]
    public async Task<IActionResult> GetRandomUser()
    {
        return Ok(await _chatService.GetRandomUser());
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateChat(NewChatDto dto)
    {
        return Ok(await _chatService.CreateChat(dto));
    }
}
