using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Commands;

namespace GP.Microservices.Common.ServiceClients
{
    public interface IRemarkServiceClient
    {
        Task<Response<ActivityDto>> AddActivityAsync(AddActivity command);

        Task<Response<CommentDto>> AddCommentAsync(AddComment command);

        Task<Response<ImageDto>> AddImageAsync(AddImage command);

        Task<Response<RemarkDto>> CancelRemarkAsync(CancelRemark command);

        Task<Response<RemarkDto>> CreateRemarkAsync(CreateRemark command);

        Task<Response<RemarkDto>> DeleteRemarkAsync(DeleteRemark command);

        Task<Response<RemarkDto>> GetRemarkAsync(Guid remarkId);

        Task<Response<IEnumerable<ActivityTypeDto>>> GetActivityTypesAsync();
            
        Task<Response<IEnumerable<ActivityDto>>> GetRemarkAcivitiesAsync(Guid remarkId);

        Task<Response<IEnumerable<RemarkCategoryDto>>> GetRemarkCategoriesAsync();

        Task<Response<IEnumerable<CommentDto>>> GetRemarkCommentsAsync(Guid remarkId);

        Task<Response<IEnumerable<ImageDto>>> GetRemarkImagesAsync(Guid remarkId); 
            
        Task<Response<CommentDto>> RemoveCommentAsync(RemoveComment command);

        Task<Response<ImageDto>> RemoveImageAsync(RemoveImage command);

        Task<Response<RemarkDto>> ResolveRemarkAsync(ResolveRemark command);
    }
}