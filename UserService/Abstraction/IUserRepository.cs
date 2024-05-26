using UserService.DTO;
using UserService.Model;

namespace UserService.Abstraction;

public interface IUserRepository
{
    public void UserAdd(string email, string password, RoleId roleId);
    public RoleType UserCheck(string name, string password);
}