using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Remarks.Domain.Models;

namespace GP.Microservices.Remarks.Domain.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAsync();

        Task<Category> GetAsync(Guid id);

        Task<Category> CreateAsync(Category category);
    }
}