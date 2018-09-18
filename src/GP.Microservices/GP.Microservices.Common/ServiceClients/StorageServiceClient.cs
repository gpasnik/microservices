using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using Microsoft.Extensions.Configuration;

namespace GP.Microservices.Common.ServiceClients
{
    public class StorageServiceClient : ServiceClientBase, IStorageServiceClient
    {
        public StorageServiceClient(HttpClient httpClient, IConfiguration config) 
            : base(httpClient, config)
        {
        }

        public async Task<Response<RemarkDto>> GetRemarkAsync(Guid id)
            => await GetAsync<RemarkDto>($"api/remarks/{id}");

        public async Task<Response<IEnumerable<RemarkCategoryDto>>> GetCategoriesAsync()
            => await GetAsync<IEnumerable<RemarkCategoryDto>>("api/remarks/categories");

        public async Task<Response<IEnumerable<ActivityTypeDto>>> GetActivitiesAsync()
            => await GetAsync<IEnumerable<ActivityTypeDto>>("api/remarks/activities");

        public async Task<Response<IEnumerable<RemarkDto>>> BrowseRemarksAsync(BrowseRemarks query)
            => await GetAsync<IEnumerable<RemarkDto>>("api/remarks");

        public async Task<Response<UserDto>> GetUserAsync(string username)
            => await GetAsync<UserDto>($"api/users/{username}");

        public async Task<Response<UserDto>> GetUserAsync(Guid id)
            => await GetAsync<UserDto>($"api/users/{id}");

        public async Task<Response<IEnumerable<UserDto>>> BrowseUsersAsync(BrowseUsers query)
            => await GetAsync<IEnumerable<UserDto>>("api/users");

        public async Task<Response<IEnumerable<StatisticsDto>>> BrowseStatisicsAsync(BrowseStatistics query)
            => await GetAsync<IEnumerable<StatisticsDto>>("api/statistics");
    }
}