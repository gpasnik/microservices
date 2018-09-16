 using System;
 using System.Threading.Tasks;
 using GP.Microservices.Common.Dto;
 using GP.Microservices.Common.Messages.Users.Events;
 using GP.Microservices.Common.ServiceClients;
 using GP.Microservices.Storage.Domain.Repositories;
 using MassTransit;

namespace GP.Microservices.Storage.Handlers
{
    /// <summary>
    /// User messages consumer
    /// </summary>
    public class UserEventHandler : 
        IConsumer<UserActivated>,
        IConsumer<UserBlocked>,
        IConsumer<UserDeleted>,
        IConsumer<UserRegistered>,
        IConsumer<UserUnblocked>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserServiceClient _serviceClient;

        /// <inheritdoc />
        public UserEventHandler(
            IUserRepository userRepository,
            IUserServiceClient serviceClient)
        {
            _userRepository = userRepository;
            _serviceClient = serviceClient;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<UserActivated> context)
        {
            await UpdateStorageEntity(context.Message.Id);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<UserBlocked> context)
        {
            await UpdateStorageEntity(context.Message.Id);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<UserDeleted> context)
        {
            await UpdateStorageEntity(context.Message.Id);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            await UpdateStorageEntity(context.Message.Id);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<UserUnblocked> context)
        {
            await UpdateStorageEntity(context.Message.Id);
        }

        private async Task<UserDto> GetRemarkAsync(Guid id)
            => await _serviceClient
                .GetUserAsync(id)
                .OrFailAsync();

        private async Task UpdateStorageEntity(Guid remarkId)
        {
            var remark = await GetRemarkAsync(remarkId);
            await _userRepository.UpsertAsync(remark);
        }
    }
}