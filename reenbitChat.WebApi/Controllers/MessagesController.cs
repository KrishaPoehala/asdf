using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reenbitChat.Common.Dtos.MessageDtos;
using reenbitChat.Domain.Services;

namespace reenbitChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendMessage(NewMessageDto dto)
        {
            await _messageService.SendMessage(dto);
            return Ok();
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditMessage(EditMessageDto dto)
        {
            await _messageService.EditMessage(dto);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}/{isDeleteOnlyForSender}")]
        public async Task<IActionResult> DeleteMessage(int id, bool isDeleteOnlyForSender)
        {
            await _messageService.DeleteMessage(id, isDeleteOnlyForSender);
            return Ok();
        }
    }
}
