﻿using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Storage.Domain.Repositories;
using MassTransit;

namespace GP.Microservices.Storage.Handlers
{
    /// <inheritdoc />
    public class RemarkCommentAddedConsumer : IConsumer<CommentAdded>
    {
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _serviceClient;

        /// <inheritdoc />
        public RemarkCommentAddedConsumer(IRemarkRepository remarkRepository,
            IRemarkServiceClient serviceClient)
        {
            _remarkRepository = remarkRepository;
            _serviceClient = serviceClient;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<CommentAdded> context)
        {
            var remark = await _serviceClient
                .GetRemarkAsync(context.Message.RemarkId)
                .OrFailAsync();
            await _remarkRepository.UpsertAsync(remark);
        }
    }
}