using System;
using System.Net.Http;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Users.Commands;
using Microsoft.Extensions.Configuration;

namespace GP.Microservices.Common.ServiceClients
{
    public class UserServiceClient : ServiceClientBase, IUserServiceClient
    {
        public UserServiceClient(HttpClient httpClient, IConfiguration config)
         :base(httpClient, config)
        {
        }

        public async Task<Response<UserDto>> ActivateUserAsync(ActivateUser command)
            => await PutAsync<UserDto>($"users/{command.Username}/activate", command);

        public async Task<Response<UserDto>> BlockUserAsync(BlockUser command)
            => await PutAsync<UserDto>($"users/{command.Username}/block", command);

        public async Task<Response<UserDto>> ChangeUserPasswordAsync(ChangeUserPassword command)
            => await PutAsync<UserDto>($"users/{command.Username}/password/change", command);

        public async Task<Response<UserDto>> DeleteUserAsync(DeleteUser command)
            => await DeleteAsync<UserDto>($"users/{command.Username}");

        public async Task<Response<UserDto>> GetUserAsync(string username)
            => await GetAsync<UserDto>($"users/{username}");

        public async Task<Response<UserDto>> GetUserAsync(Guid id)
            => await GetAsync<UserDto>($"users/{id}");

        public async Task<Response<UserDto>> RegisterUserAsync(RegisterUser command)
            => await PostAsync<UserDto>("users", command);

        public async Task<Response<UserDto>> ResetUserPasswordAsync(ResetUserPassword command)
            => await PutAsync<UserDto>($"users/{command.Username}/password/reset", command);

        public async Task<Response<UserDto>> UnblockUserAsync(UnblockUser command)
            => await PutAsync<UserDto>($"users/{command.Username}/unblock", command);
    }
}