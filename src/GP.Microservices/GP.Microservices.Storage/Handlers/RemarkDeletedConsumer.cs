using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Storage.Domain.Repositories;
using MassTransit;

namespace GP.Microservices.Storage.Handlers
{
    /// <inheritdoc />
    public class RemarkDeletedConsumer : IConsumer<RemarkDeleted>
    {
        private readonly IRemarkRepository _remarkRepository;

        /// <inheritdoc />
        public RemarkDeletedConsumer(IRemarkRepository remarkRepository)
        {
            _remarkRepository = remarkRepository;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<RemarkDeleted> context)
        {
            await _remarkRepository.DeleteAsync(new RemarkDto { Id = context.Message.RemarkId });
        }
    }
}