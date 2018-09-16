using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Extensions;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Domain.Repositories.Queries
{
    public static class RemarkQueries
    {
        public static IMongoCollection<RemarkDto> Remarks(this IMongoDatabase database)
            => database.GetCollection<RemarkDto>();

        public static async Task<RemarkDto> GetByIdAsync(this IMongoCollection<RemarkDto> remarks, Guid id)
        {
            return await remarks.Find(x => x.Id == id).SingleOrDefaultAsync();
        }

        public static async Task<IEnumerable<RemarkDto>> QueryAsync(this IMongoCollection<RemarkDto> remarks,
            BrowseRemarks query)
        {
            if (query.Page <= 0)
            {
                query.Page = 1;
            }
            if (query.Results <= 0)
            {
                query.Results = 100;
            }

            var filterBuilder = new FilterDefinitionBuilder<RemarkDto>();
            var filter = FilterDefinition<RemarkDto>.Empty;

            var filteredRemarks = remarks.Find(filter);
            var findResults = filteredRemarks
                .Skip(query.Results * (query.Page - 1))
                .Limit(query.Results);

            var result = await findResults.ToListAsync();

            return result;
        }
    }
}