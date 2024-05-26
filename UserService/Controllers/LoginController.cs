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
    [Route("[controller]")]
    public class LoginController(IUserRepository userRepository, ITokenService tokenService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginViewModel userLogin)
        {
            try
            {
                var roleId = userRepository.UserCheck(userLogin.Email, userLogin.Password);

                var user = new LoginViewModel() { Email = userLogin.Email, UserRole = roleId };

                var token = tokenService.GenerateToken(user);
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

    }
}
