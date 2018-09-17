using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Storage.Extensions;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public class ActivityTypeRepository : IActivityTypeRepository
    {
        private readonly IMongoDatabase _database;

        public ActivityTypeRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<ActivityTypeDto>> GetAsync()
        {
            var collection = _database.GetCollection<ActivityTypeDto>();

            return await collection.Find(FilterDefinition<ActivityTypeDto>.Empty).ToListAsync();
        }

        public async Task<ActivityTypeDto> GetAsync(Guid id)
        {
            var collection = _database.GetCollection<ActivityTypeDto>();

            return await collection.Find(x => x.Id == id).SingleOrDefaultAsync();
        }
    }
}