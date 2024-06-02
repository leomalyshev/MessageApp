using UserService.DTO;
using UserService.Model;

namespace UserService.Abstraction;

public interface IUserRepository
{
    public void UserAdd(string email, string password, RoleId roleId);
    public User UserCheck(string name, string password);
    public IEnumerable<LoginViewModel> GetUsers();
    public void UserDelete(string email);
}