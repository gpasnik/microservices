using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public interface IActivityTypeRepository
    {
        Task<IEnumerable<ActivityTypeDto>> GetAsync();

        Task<ActivityTypeDto> GetAsync(Guid id);
    }
}