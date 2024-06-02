using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Abstraction;
using UserService.Context;
using UserService.DTO;
using UserService.Model;

namespace UserService.Repository
{
    public class UserRepository(IMapper mapper, UserContext context) : IUserRepository
    {
        public void UserAdd(string email, string password, RoleId roleId)
        {
            if (roleId == RoleId.Admin)
            {
                var count = context.Users.Count(x => x.RoleId == RoleId.Admin);
                if (count > 0)
                {
                    throw new Exception("Admin already exists");
                }
            }

            var checkUser = context.Users.FirstOrDefault(user => user.Email == email);

            if (checkUser != null) throw new Exception("User already exists");

            var user = new User()
            {
                Email = email,
                RoleId = roleId,
                Salt = new byte[16]
            };

            new Random().NextBytes(user.Salt);
            var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();

            SHA512 shaM = new SHA512Managed();
            user.Password = shaM.ComputeHash(data);
            context.Add(user);
            context.SaveChanges();
        }

        public User UserCheck(string name, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == name);
            if (user == null)
            {
                throw new System.Exception("User not found");
            }

            var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var hash = shaM.ComputeHash(data);

            if (hash.SequenceEqual(user.Password))
            {
                return user;
            }

            throw new Exception("Wrong password");
        }

        public IEnumerable<LoginViewModel> GetUsers()
        {
            return context.Users.Include(x => x.Role).Select(x => mapper.Map<LoginViewModel>(x));
        }

        public void UserDelete(string email)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (user.RoleId == RoleId.Admin)
            {
                var count = context.Users.Count(x => x.RoleId == RoleId.Admin);
                if (count == 1)
                {
                    throw new Exception("You can't delete the last admin");
                }
            }

            

            context.Remove(user);
            context.SaveChanges();
        }

    }
}

