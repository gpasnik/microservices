using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Users.Domain.Models;

namespace GP.Microservices.Users.Domain.Services
{
    public interface IUserService
    {
        Task<User> GetAsync(string username);

        Task<User> GetAsync(Guid id);

        Task<User> AuthorizeAsync(AuthorizeUser command);

        Task<User> RegisterAsync(string username, string password, string email, string name, string lastname);

        Task<User> ActivateAsync(string activationToken);

        Task<User> DeleteAsync(string username);

        Task<User> BlockAsync(string username);

        Task<User> UnblockAsync(string username);

        Task<User> ChangePasswordAsync(string username, string newPassword);

        Task<User> ResetPasswordAsync(string username);
    }
}