using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Remarks.Data;
using GP.Microservices.Remarks.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Remarks.Domain.Services
{
    public class ActivityService : IActivityService
    {
        private readonly RemarksContext _context;

        public ActivityService(RemarksContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActivityType>> GetAsync()
        {
            return await _context.ActivityTypes.ToListAsync();
        }

        public async Task<ActivityType> GetAsync(Guid id)
        {
            return await _context.ActivityTypes.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ActivityType> CreateAsync(ActivityType activityType)
        {
            var entry = await _context.ActivityTypes.AddAsync(activityType);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}