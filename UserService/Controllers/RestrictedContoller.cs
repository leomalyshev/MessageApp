using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.Model;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestrictedContoller : ControllerBase
    {
        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hello, {currentUser}! You are an admin.");
        }

        [HttpGet]
        [Route("Users")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult UserEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hello, {currentUser}! You are an user.");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                // Получаем значение Role из утверждений
                var roleValue = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                RoleId roleId;
                if (Enum.TryParse(roleValue, out roleId))
                {
                    var role = new Role
                    {
                        RoleId = roleId,
                        Name = roleValue
                    };

                    return new User
                    {
                        Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                        Role = role
                    };
                }
            }
            return null;
        }
    }
}
