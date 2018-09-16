using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> BrowseAsync(BrowseUsers query);

        Task<UserDto> GetAsync(string username);

        Task<UserDto> GetAsync(Guid id);

        Task UpsertAsync(UserDto user);

        Task DeleteAsync(UserDto user);
    }
}