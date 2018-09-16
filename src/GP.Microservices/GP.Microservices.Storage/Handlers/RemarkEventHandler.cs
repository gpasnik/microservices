using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Storage.Domain.Repositories;
using MassTransit;

namespace GP.Microservices.Storage.Handlers
{
    /// <summary>
    /// Remark messages consumer
    /// </summary>
    public class RemarkEventHandler :
        IConsumer<ActivityAdded>,
        IConsumer<CommentAdded>,
        IConsumer<CommentRemoved>,
        IConsumer<ImageAdded>,
        IConsumer<ImageRemoved>,
        IConsumer<RemarkCanceled>,
        IConsumer<RemarkCreated>,
        IConsumer<RemarkDeleted>,
        IConsumer<RemarkResolved>
    {
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _serviceClient;

        /// <inheritdoc />
        public RemarkEventHandler(
            IRemarkRepository remarkRepository,
            IRemarkServiceClient serviceClient)
        {
            _remarkRepository = remarkRepository;
            _serviceClient = serviceClient;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<ActivityAdded> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<CommentAdded> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<CommentRemoved> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<ImageAdded> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<ImageRemoved> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<RemarkCanceled> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<RemarkCreated> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<RemarkDeleted> context)
        {
            await _remarkRepository.DeleteAsync(new RemarkDto {Id = context.Message.RemarkId});
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<RemarkResolved> context)
        {
            await UpdateStorageEntity(context.Message.RemarkId);
        }

        private async Task<RemarkDto> GetRemarkAsync(Guid id) 
            => await _serviceClient
                .GetRemarkAsync(id)
                .OrFailAsync();

        private async Task UpdateStorageEntity(Guid remarkId)
        {
            var remark = await GetRemarkAsync(remarkId);
            await _remarkRepository.UpsertAsync(remark);
        }
    }
}