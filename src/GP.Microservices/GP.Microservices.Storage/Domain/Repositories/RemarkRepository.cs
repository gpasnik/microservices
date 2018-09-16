using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Domain.Repositories.Queries;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public class RemarkRepository : IRemarkRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<RemarkDto>> BrowseAsync(BrowseRemarks query)
        {
            return await _database.Remarks().QueryAsync(query);
        }

        public async Task<RemarkDto> GetAsync(Guid id)
        {
            return await _database.Remarks().GetByIdAsync(id);
        }

        public async Task UpsertAsync(RemarkDto remark)
        {
            await _database.Remarks().ReplaceOneAsync(x => x.Id == remark.Id, remark, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public async Task DeleteAsync(RemarkDto remark)
        {
            await _database.Remarks().DeleteOneAsync(x => x.Id == remark.Id);
        }
    }
}