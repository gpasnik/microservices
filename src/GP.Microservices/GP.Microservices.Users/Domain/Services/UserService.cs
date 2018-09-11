using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Exceptions;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Users.Data;
using GP.Microservices.Users.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Users.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;

        public UserService(UsersContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
            if (user == null)
                throw new ServiceException(new ServiceError("User does not exist", 404));

            return user;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ServiceException(new ServiceError("User does not exist", 404));

            return user;
        }

        public async Task<User> AuthorizeAsync(AuthorizeUser command)
        {
            var user = await GetAsync(command.Username);

            if (user.Password != command.Password)
                throw new ServiceException(new ServiceError("Invalid password", 401));

            return user;
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