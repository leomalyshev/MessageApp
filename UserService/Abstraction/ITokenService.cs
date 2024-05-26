using UserService.DTO;
using UserService.Model;

namespace UserService.Abstraction;

public interface ITokenService
{
    public string GenerateToken(LoginViewModel loginViewModel);
}