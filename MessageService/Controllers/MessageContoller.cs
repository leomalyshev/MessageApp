using MessageService.Abstraction;
using MessageService.DTO;
using MessageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [Authorize] // Требуется аутентификация для доступа к этому контроллеру
    [Route("api/[controller]")]
    [ApiController]
    public class MessageContoller : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageContoller(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet(template: "getmessages")]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            var userId = Guid.Parse(User.Identity.Name);
            var messages = _messageRepository.GetMessageForUser(userId);
            return Ok(messages);
        }

        [HttpPost(template: "sendmessage")]
        public ActionResult<IEnumerable<Message>> SendMessage(MessageViewModel messageViewModel)
        {
            var senderId = Guid.Parse(User.Identity.Name);
            var message = new Message
            {
                SenderId = senderId,
                RecipientId = messageViewModel.RecipientId,
                Content = messageViewModel.Content,
            };
            _messageRepository.SendMessage(message);
            return Ok();
        }
    }
}
