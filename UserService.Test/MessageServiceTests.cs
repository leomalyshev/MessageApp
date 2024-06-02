using System.Collections;
using System.Security.Claims;
using MessageService.Abstraction;
using MessageService.Controllers;
using MessageService.DTO;
using MessageService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Abstraction;
using UserService.Controllers;
using UserService.DTO;
using UserService.Model;
using UserService.Test;

namespace MessageService.Test
{
    public class MessageTests
    {
        private MessageController _messageController;
        private Mock<IMessageRepository> _messageRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();

            _messageController = new MessageController(_messageRepositoryMock.Object);
        }

        public List<Message> GetMessages()
        {
            List<Message> messages =
            [
                new Message()
                {
                    Content = "Hello, person!",
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    RecipientId = Guid.NewGuid(),
                    SenderId = Guid.NewGuid()
                },

                new Message()
                {
                    Content = "Hello, people!",
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    RecipientId = Guid.NewGuid(),
                    SenderId = Guid.NewGuid()
                },
            ];
            return messages;
        }

        [Test]
        public void GetMessages_ValidUserId_ReturnsMessages()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var messages = GetMessages();

            _messageRepositoryMock.Setup(repo => repo.GetMessageForUser(userId)).Returns(messages);

            // Создаем фейковый контекст пользователя с нужным claim
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId.ToString())
            }, "mock"));

            _messageController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = _messageController.GetMessages();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(messages, okResult.Value);
        }

        [Test]
        public void SendMessage_ValidUser_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var messageViewModel = new MessageViewModel
            {
                Content = "Hello!",
                RecipientId = Guid.NewGuid()
            };

            _messageRepositoryMock.Setup(repo => repo.SendMessage(It.IsAny<Message>()));

            // Создаем фейковый контекст пользователя с нужным claim
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId.ToString())
            }, "mock"));

            _messageController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = _messageController.SendMessage(messageViewModel);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);

            // Проверяем, что метод SendMessage был вызван с ожидаемым сообщением
            _messageRepositoryMock.Verify(repo => repo.SendMessage(It.Is<Message>(msg =>
                msg.Content == messageViewModel.Content &&
                msg.SenderId == userId &&
                msg.RecipientId == messageViewModel.RecipientId)), Times.Once);
        }
    }
}