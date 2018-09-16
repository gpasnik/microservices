using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Remarks.Domain.Models;

namespace GP.Microservices.Remarks.Domain.Services
{
    public interface IRemarkService
    {
        Task<IEnumerable<Remark>> BrowseAsync(BrowseRemarks query);

        Task<Remark> GetAsync(Guid id);

        Task<Remark> CreateAsync(CreateRemark command);

        Task<Remark> ResolveAsync(ResolveRemark command);

        Task<Remark> CancelAsync(CancelRemark command);

        Task<Remark> DeleteAsync(DeleteRemark command);

        Task<IEnumerable<Activity>> GetActivitiesAsync(Guid remarkId);

        Task<Activity> AddActivityAsync(AddActivity command);
        
        Task<IEnumerable<Comment>> GetCommentsAsync(Guid remarkId);

        Task<Comment> AddCommentAsync(AddComment command);
        
        Task<Comment> RemoveCommentAsync(RemoveComment command);

        Task<IEnumerable<Image>> GetImagessAsync(Guid remarkId);

        Task<Image> AddImageAsync(AddImage command);

        Task<Image> RemoveImageAsync(RemoveImage command);
    }
}