using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Exceptions;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.Messages.Users.Events;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Users.Data;
using GP.Microservices.Users.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Users.Domain.Services
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly UsersContext _context;
        private readonly IBus _bus;

        /// <inheritdoc />
        public UserService(UsersContext context,
            IBus bus)
        {
            _context = context;
            _bus = bus;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> BrowseAsync(BrowseUsers query)
        {
            return await _context.Users.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User> GetAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
            if (user == null)
                throw new ServiceException(new ServiceError("User does not exist", 404));

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetAsync(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ServiceException(new ServiceError("User does not exist", 404));

            return user;
        }

        /// <inheritdoc />
        public async Task<User> AuthorizeAsync(AuthorizeUser command)
        {
            var user = await GetAsync(command.Username);

            if (user.Password != command.Password)
                throw new ServiceException(new ServiceError("Invalid password", 401));

            return user;
        }

        /// <inheritdoc />
        public async Task<User> RegisterAsync(string username, string password, string email, string name,
            string lastname)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = password,
                Email = email,
                Name = name,
                Lastname = lastname,
                Status = UserStatus.Active,
                DateCreated = DateTime.UtcNow
            };
            var entry = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await _bus.Publish(new UserRegistered
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            });

            return entry.Entity;
        }

        /// <inheritdoc />
        public async Task<User> ActivateAsync(string activationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<User> DeleteAsync(string username)
        {
            var user = await GetAsync(username);

            user.Username = $"{user.Username}-{user.Id:N}";
            user.Email = user.Id.ToString("N");
            user.Status = UserStatus.Deleted;
            await _context.SaveChangesAsync();

            await _bus.Publish(new UserDeleted
            {
                Id = user.Id,
                Username = user.Username
            });

            return user;
        }

        /// <inheritdoc />
        public async Task<User> BlockAsync(string username)
        {
            var user = await GetAsync(username);

            user.Status = UserStatus.Blocked;
            await _context.SaveChangesAsync();

            await _bus.Publish(new UserBlocked
            {
                Id = user.Id,
                Username = user.Username
            });

            return user;
        }

        /// <inheritdoc />
        public async Task<User> UnblockAsync(string username)
        {
            var user = await GetAsync(username);

            user.Status = UserStatus.Active;
            await _context.SaveChangesAsync();

            await _bus.Publish(new UserUnblocked
            {
                Id = user.Id,
                Username = user.Username
            });

            return user;
        }

        /// <inheritdoc />
        public async Task<User> ChangePasswordAsync(string username, string newPassword)
        {
            var user = await GetAsync(username);

            user.Password = newPassword;
            await _context.SaveChangesAsync();

            return user;
        }

        /// <inheritdoc />
        public async Task<User> ResetPasswordAsync(string username)
        {
            var user = await GetAsync(username);

            user.Password = Guid.NewGuid().ToString("N");
            await _context.SaveChangesAsync();

            return user;
        }
    }
}