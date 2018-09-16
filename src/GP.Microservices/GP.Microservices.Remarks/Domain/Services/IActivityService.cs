using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Remarks.Domain.Models;

namespace GP.Microservices.Remarks.Domain.Services
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityType>> GetAsync();

        Task<ActivityType> GetAsync(Guid id);

        Task<ActivityType> CreateAsync(ActivityType activityType);
    }
}