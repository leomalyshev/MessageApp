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
    public class UserTests
    {
        private LoginController _loginController;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITokenService> _tokenServiceMock;
        
        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            _loginController = new LoginController(_userRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userLogin = new LoginViewModel { Email = "test@example.com", Password = "password123" };
            var user = new User { 
                Email = userLogin.Email, 
                RoleId = RoleId.User, 
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            };
            var expectedToken = "token"; // Пример токена
            _userRepositoryMock.Setup(repo => repo.UserCheck(userLogin.Email, userLogin.Password)).Returns(user);
            _tokenServiceMock.Setup(service => service.GenerateToken(It.IsAny<LoginViewModel>())).Returns(expectedToken);

            // Act
            var result = _loginController.Login(userLogin);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedToken, okResult.Value);
        }

        [Test]
        public void AddAdmin_ValidCountAdmins()
        {
            // Arrange
            var userLogin = new LoginViewModel { Email = "admin2@example.com", Password = "password123" };
            var userRepo = new MockUserRepository();
            //Act
            userRepo.UserAdd(userLogin.Email, userLogin.Password, RoleId.Admin);

            var getAdmins = userRepo.GetUsers().Where(x => x.UserRole == RoleType.Admin);

            // Assert
            Assert.AreEqual(2, getAdmins.Count());
        }

        [Test]
        public void GetUsers_ValidCountUsers()
        {
            // Arrange
            var userLogin = new LoginViewModel { Email = "admin2@example.com", Password = "password123" };
            var userRepo = new MockUserRepository();
            //Act
            userRepo.UserAdd(userLogin.Email, userLogin.Password, RoleId.Admin);

            // Assert
            Assert.AreEqual(4, userRepo.GetUsers().Count());
        }

        [Test]
        public void DeleteUser_ValidCountUsers()
        {
            // Arrange
            var userLogin = new LoginViewModel { Email = "admin@example.com", Password = "password123" };
            var userRepo = new MockUserRepository();
            //Act
            userRepo.UserDelete("admin@example.com");

            // Assert
            Assert.AreEqual(2, userRepo.GetUsers().Count());
        }


    }
}