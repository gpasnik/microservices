using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Commands;
using Microsoft.Extensions.Configuration;

namespace GP.Microservices.Common.ServiceClients
{
    public class RemarkServiceClient : ServiceClientBase, IRemarkServiceClient
    {
        public RemarkServiceClient(HttpClient httpClient, IConfiguration config) 
            : base(httpClient, config)
        {
        }

        public async Task<Response<ActivityDto>> AddActivityAsync(AddActivity command)
            => await PostAsync<ActivityDto>($"api/remarks/{command.RemarkId}/activities", command);

        public async Task<Response<CommentDto>> AddCommentAsync(AddComment command)
            => await PostAsync<CommentDto>($"api/remarks/{command.RemarkId}/comments", command);

        public async Task<Response<ImageDto>> AddImageAsync(AddImage command)
            => await PostAsync<ImageDto>($"api/remarks/{command.RemarkId}/images", command);

        public async Task<Response<RemarkDto>> CancelRemarkAsync(CancelRemark command)
            => await PutAsync<RemarkDto>($"api/remarks/{command.RemarkId}/cancel", command);

        public async Task<Response<RemarkDto>> CreateRemarkAsync(CreateRemark command)
            => await PostAsync<RemarkDto>("api/remarks", command);

        public async Task<Response<RemarkDto>> DeleteRemarkAsync(DeleteRemark command)
            => await DeleteAsync<RemarkDto>($"api/remarks/{command.RemarkId}");

        public async Task<Response<RemarkDto>> GetRemarkAsync(Guid remarkId)
            => await GetAsync<RemarkDto>($"api/remarks/{remarkId}");

        public async Task<Response<IEnumerable<ActivityTypeDto>>> GetActivityTypesAsync()
            => await GetAsync<IEnumerable<ActivityTypeDto>>("api/activities");

        public async Task<Response<IEnumerable<ActivityDto>>> GetRemarkAcivitiesAsync(Guid remarkId)
            => await GetAsync<IEnumerable<ActivityDto>>($"api/remarks/{remarkId}/activities");

        public async Task<Response<IEnumerable<RemarkCategoryDto>>> GetRemarkCategoriesAsync()
            => await GetAsync<IEnumerable<RemarkCategoryDto>>("api/categories");

        public async Task<Response<IEnumerable<CommentDto>>> GetRemarkCommentsAsync(Guid remarkId)
            => await GetAsync<IEnumerable<CommentDto>>($"api/remarks/{remarkId}/comments");

        public async Task<Response<IEnumerable<ImageDto>>> GetRemarkImagesAsync(Guid remarkId)
            => await GetAsync<IEnumerable<ImageDto>>($"api/remarks/{remarkId}/images");

        public async Task<Response<CommentDto>> RemoveCommentAsync(RemoveComment command)
            => await DeleteAsync<CommentDto>($"api/remarks/{command.RemarkId}/comments/{command.CommentId}");

        public async Task<Response<ImageDto>> RemoveImageAsync(RemoveImage command)
            => await DeleteAsync<ImageDto>($"api/remarks/{command.RemarkId}/images/{command.ImageId}");

        public async Task<Response<RemarkDto>> ResolveRemarkAsync(ResolveRemark command)
            => await PutAsync<RemarkDto>($"api/remarks/{command.RemarkId}/resolve", command);
    }
}