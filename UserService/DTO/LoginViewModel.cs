using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public RoleType UserRole { get; set; } = RoleType.User;
    }
}
