using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Abstraction;
using UserService.DTO;
using UserService.Model;
using UserService.Repository;

namespace UserService.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController(IUserRepository userRepository, ITokenService tokenService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] LoginViewModel userLogin)
        {
            try
            {
                var user = userRepository.UserCheck(userLogin.Email, userLogin.Password);

                var roleId = (RoleType)user.RoleId;

                var loginUser = new LoginViewModel()
                {
                    Email = userLogin.Email, UserRole = roleId, Id = user.Id
                };

                var token = tokenService.GenerateToken(loginUser);
                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("addadmin")]
        //"email": "admin@admin.com",
        //"password": "admin",
        public ActionResult AddAdmin([FromBody] LoginViewModel userLogin)
        {
            try
            {
                userRepository.UserAdd(userLogin.Email, userLogin.Password, RoleId.Admin);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("adduser")]
        [Authorize(Roles = "Admin")]
        public ActionResult AddUser([FromBody] LoginViewModel userLogin)
        {
            try
            {
                userRepository.UserAdd(userLogin.Email, userLogin.Password, RoleId.User);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("getusers")]
        [Authorize(Roles = "Admin, User")]
        public IEnumerable<LoginViewModel> GetUsers()
        {
            return userRepository.GetUsers();
        }

        [HttpPost]
        [Route("deleteuser")]
        [Authorize(Roles = "Admin")]
        public ActionResult UserDelete([FromBody] LoginViewModel userLogin)
        {
            try
            {
                userRepository.UserDelete(userLogin.Email);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }
    }
}