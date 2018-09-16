using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Extensions;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Domain.Repositories.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<UserDto> Users(this IMongoDatabase database)
            => database.GetCollection<UserDto>();

        public static async Task<UserDto> GetByIdAsync(this IMongoCollection<UserDto> users, Guid id)
        {
            return await users.Find(x => x.Id == id).SingleOrDefaultAsync();
        }

        public static async Task<IEnumerable<UserDto>> QueryAsync(this IMongoCollection<UserDto> users,
            BrowseUsers query)
        {
            if (query.Page <= 0)
            {
                query.Page = 1;
            }
            if (query.Results <= 0)
            {
                query.Results = 100;
            }

            var filterBuilder = new FilterDefinitionBuilder<UserDto>();
            var filter = FilterDefinition<UserDto>.Empty;

            var filteredRemarks = users.Find(filter);
            var findResults = filteredRemarks
                .Skip(query.Results * (query.Page - 1))
                .Limit(query.Results);

            var result = await findResults.ToListAsync();

            return result;
        }
    }
}