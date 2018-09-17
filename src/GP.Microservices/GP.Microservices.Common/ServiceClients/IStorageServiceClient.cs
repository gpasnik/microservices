using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;

namespace GP.Microservices.Common.ServiceClients
{
    public interface IStorageServiceClient
    {
        Task<Response<RemarkDto>> GetRemarkAsync(Guid id);

        Task<Response<IEnumerable<RemarkCategoryDto>>> GetCategoriesAsync();

        Task<Response<IEnumerable<ActivityTypeDto>>> GetActivitiesAsync();

        Task<Response<IEnumerable<RemarkDto>>> BrowseRemarksAsync(BrowseRemarks query);

        Task<Response<UserDto>> GetUserAsync(string username);

        Task<Response<UserDto>> GetUserAsync(Guid id);

        Task<Response<IEnumerable<UserDto>>> BrowseUsersAsync(BrowseUsers query);

        Task<Response<IEnumerable<StatisticsDto>>> BrowseStatisicsAsync(BrowseStatistics query);
    }
}