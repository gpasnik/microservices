using System;
using System.Threading.Tasks;
using GP.Microservices.Users.Data;
using GP.Microservices.Users.Domain.Models;

namespace GP.Microservices.Users.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;

        public UserService(UsersContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(string username, string password, string email, string name,
            string lastname)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ActivateAsync(string activationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> DeleteAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> BlockAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UnblockAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ChangePasswordAsync(string username, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ResetPasswordAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}