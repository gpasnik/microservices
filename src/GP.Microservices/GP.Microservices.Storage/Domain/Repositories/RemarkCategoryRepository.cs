using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Storage.Extensions;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public class RemarkCategoryRepository : IRemarkCategoryRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkCategoryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<RemarkCategoryDto>> GetAsync()
        {
            var collection = _database.GetCollection<RemarkCategoryDto>();

            return await collection.Find(FilterDefinition<RemarkCategoryDto>.Empty).ToListAsync();
        }

        public async Task<RemarkCategoryDto> GetAsync(Guid id)
        {
            var collection = _database.GetCollection<RemarkCategoryDto>();

            return await collection.Find(x => x.Id == id).SingleOrDefaultAsync();
        }
    }
}