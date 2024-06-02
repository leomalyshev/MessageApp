using MessageService.Abstraction;
using MessageService.DTO;
using MessageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessageService.Controllers
{
    [ApiController]
    [Authorize] // Требуется аутентификация для доступа к этому контроллеру
    [Route("message")]
    public class MessageController(IMessageRepository messageRepository) : ControllerBase
    {
        [HttpGet(template: "getmessages")]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            try
            {
                var userIdClaimValue = User.FindFirstValue("Id");
                if (userIdClaimValue != null)
                {
                    var userId = Guid.Parse(userIdClaimValue);
                    var messages = messageRepository.GetMessageForUser(userId);
                    return Ok(messages);
                }

                return BadRequest("User ID claim is missing in the token.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(template: "sendmessage")]
        public ActionResult SendMessage(MessageViewModel messageViewModel)
        {
            try
            {
                var userIdClaimValue = User.FindFirstValue("Id");
                if (userIdClaimValue != null)
                {
                    var senderId = Guid.Parse(userIdClaimValue);
                    var message = new Message
                    {
                        SenderId = senderId,
                        RecipientId = messageViewModel.RecipientId,
                        Content = messageViewModel.Content,
                    };
                    messageRepository.SendMessage(message);
                    return Ok();
                }

                return BadRequest("User ID claim is missing in the token.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
