using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Users.Events;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Storage.Domain.Repositories;
using MassTransit;

namespace GP.Microservices.Storage.Handlers
{
    /// <inheritdoc />
    public class UserRegisteredConsumer : IConsumer<UserRegistered>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserServiceClient _serviceClient;

        /// <inheritdoc />
        public UserRegisteredConsumer(
            IUserRepository userRepository,
            IUserServiceClient serviceClient)
        {
            _userRepository = userRepository;
            _serviceClient = serviceClient;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            var user = await _serviceClient
                .GetUserAsync(context.Message.Id)
                .OrFailAsync();
            await _userRepository.UpsertAsync(user);
        }
    }
}