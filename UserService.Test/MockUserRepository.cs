using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UserService.Abstraction;
using UserService.DTO;
using UserService.Model;

namespace UserService.Test
{
    public class MockUserRepository : IUserRepository
    {
        private List<LoginViewModel> _users =
        [
            new() { Email = "user1@example.com", UserRole = RoleType.User },
            new() { Email = "user2@example.com", UserRole = RoleType.User },
            new() { Email = "admin@example.com", UserRole = RoleType.Admin }
        ];

        public void UserAdd(string email, string password, RoleId roleId)
        {
            var user = new LoginViewModel() { Email = email, Password = password, UserRole = (RoleType)roleId };
            _users.Add(user);
        }

        public User UserCheck(string email, string password)
        {
            var check = _users.FirstOrDefault(x => x.Email == email);
            if (check != null)
            {
                return new User() { Email = check.Email };
            }

            return null;
        }

        public IEnumerable<LoginViewModel> GetUsers()
        {
            return _users;
        }

        public void UserDelete(string email)
        {
            var check = _users.FirstOrDefault(x => x.Email == email);
            if (check != null)
            {
                _users.Remove(check);
            }
        }
    }
}