using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Extensions;

namespace GP.Microservices.Storage.Domain.Repositories
{
    public interface IRemarkRepository
    {
        Task<IEnumerable<RemarkDto>> BrowseAsync(BrowseRemarks query);

        Task<RemarkDto> GetAsync(Guid id);

        Task UpsertAsync(RemarkDto remark);

        Task DeleteAsync(RemarkDto remark);
    }
}