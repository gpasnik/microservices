using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public interface IRemarkCategoryRepository
    {
        Task<IEnumerable<RemarkCategoryDto>> GetAsync();

        Task<RemarkCategoryDto> GetAsync(Guid id);
    }
}