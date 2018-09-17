using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Storage.Domain.Repositories;
using MassTransit;

namespace GP.Microservices.Storage.Handlers
{
    /// <inheritdoc />
    public class RemarkImageAddedConsumer : IConsumer<ImageAdded>
    {
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _serviceClient;

        /// <inheritdoc />
        public RemarkImageAddedConsumer(IRemarkRepository remarkRepository,
            IRemarkServiceClient serviceClient)
        {
            _remarkRepository = remarkRepository;
            _serviceClient = serviceClient;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<ImageAdded> context)
        {
            var remark = await _serviceClient
                .GetRemarkAsync(context.Message.RemarkId)
                .OrFailAsync();
            await _remarkRepository.UpsertAsync(remark);
        }
    }
}