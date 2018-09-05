using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Users.Commands;

namespace GP.Microservices.Common.ServiceClients
{
    public interface IUserServiceClient
    {
        Task<Response<UserDto>> ActivateUserAsync(ActivateUser command);

        Task<Response<UserDto>> BlockUserAsync(BlockUser command);

        Task<Response<UserDto>> ChangeUserPasswordAsync(ChangeUserPassword command);

        Task<Response<UserDto>> DeleteUserAsync(DeleteUser command);

        Task<Response<UserDto>> GetUserAsync(string username);

        Task<Response<UserDto>> GetUserAsync(Guid id);

        Task<Response<UserDto>> RegisterUserAsync(RegisterUser command);

        Task<Response<UserDto>> ResetUserPasswordAsync(ResetUserPassword command);

        Task<Response<UserDto>> UnblockUserAsync(UnblockUser command);
    }
}