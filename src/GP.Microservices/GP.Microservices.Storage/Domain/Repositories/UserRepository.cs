using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Domain.Repositories.Queries;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<UserDto>> BrowseAsync(BrowseUsers query)
        {
            return await _database.Users().QueryAsync(query);
        }

        public async Task<UserDto> GetAsync(string username)
        {
            return await _database.Users().Find(x => x.Username == username).SingleOrDefaultAsync();
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            return await _database.Users().GetByIdAsync(id);
        }

        public async Task UpsertAsync(UserDto user)
        {
            await _database.Users().ReplaceOneAsync(x => x.Id == user.Id, user, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public async Task DeleteAsync(UserDto user)
        {
            await _database.Users().DeleteOneAsync(x => x.Id == user.Id);
        }
    }
}