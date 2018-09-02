using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.Messages.Users.Events;
using GP.Microservices.Users.Domain;
using GP.Microservices.Users.Domain.Services;
using MassTransit;

namespace GP.Microservices.Users.Handlers
{
    public class UserCommandHandler : IConsumer<RegisterUser>,
        IConsumer<ActivateUser>,
        IConsumer<DeleteUser>,
        IConsumer<BlockUser>,
        IConsumer<UnblockUser>,
        IConsumer<ChangeUserPassword>,
        IConsumer<ResetUserPassword>
    {
        private readonly IUserService _userService;
        private readonly IBus _bus;

        public UserCommandHandler(IUserService userService,
            IBus bus)
        {
            _userService = userService;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<RegisterUser> context)
        {
            var message = context.Message;
            try
            {
                var user = await _userService.RegisterAsync(message.Username, message.Password, message.Email, message.Name,
                    message.LastName);

                var @event = new UserRegistered
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                };

                await _bus.Publish(@event);
            }
            catch (Exception)
            {
                var failedEvent = new RegisterUserFailed
                {
                    Username = message.Username
                };
                await _bus.Publish(failedEvent);
                throw;
            }
        }

        public async Task Consume(ConsumeContext<ActivateUser> context)
        {
            var message = context.Message;
            try
            {
                var user = await _userService.ActivateAsync(message.ActivationToken);

                var @event = new UserActivated
                {
                    Id = user.Id,
                    Username = user.Username
                };

                await _bus.Publish(@event);
            }
            catch (Exception ex)
            {
                var domainException = ex as UserServiceException;
                var failedEvent = new ActivateUserFailed
                {
                    Username = domainException?.Username,
                    Id = domainException?.UserId,
                    ActivationToken = message.ActivationToken
                };
                await _bus.Publish(failedEvent);
                throw;
            }
        }

        public async Task Consume(ConsumeContext<DeleteUser> context)
        {
            var message = context.Message;
            try
            {
                var user = await _userService.DeleteAsync(message.Username);

                var @event = new UserDeleted
                {
                    Id = user.Id,
                    Username = user.Username
                };

                await _bus.Publish(@event);
            }
            catch (Exception ex)
            {
                var domainException = ex as UserServiceException;
                var failedEvent = new DeleteUserFailed
                {
                    Id = domainException?.UserId,
                    Username = message.Username
                };
                await _bus.Publish(failedEvent);
                throw;
            }
        }

        public async Task Consume(ConsumeContext<BlockUser> context)
        {
            var message = context.Message;
            try
            {
                var user = await _userService.BlockAsync(message.Username);

                var @event = new UserBlocked
                {
                    Id = user.Id,
                    Username = user.Username
                };

                await _bus.Publish(@event);
            }
            catch (Exception ex)
            {
                var domainException = ex as UserServiceException;
                var failedEvent = new BlockUserFailed
                {
                    Id = domainException?.UserId,
                    Username = message.Username
                };
                await _bus.Publish(failedEvent);
                throw;
            }
        }

        public async Task Consume(ConsumeContext<UnblockUser> context)
        {
            var message = context.Message;
            try
            {
                var user = await _userService.UnblockAsync(message.Username);

                var @event = new UserUnblocked
                {
                    Id = user.Id,
                    Username = user.Username
                };

                await _bus.Publish(@event);
            }
            catch (Exception ex)
            {
                var domainException = ex as UserServiceException;
                var failedEvent = new UnblockUserFailed
                {
                    Id = domainException?.UserId,
                    Username = message.Username
                };
                await _bus.Publish(failedEvent);
                throw;
            }
        }

        public async Task Consume(ConsumeContext<ChangeUserPassword> context)
        {
            var message = context.Message;

            await _userService.ChangePasswordAsync(message.Username, message.NewPassword);
        }

        public async Task Consume(ConsumeContext<ResetUserPassword> context)
        {
            var nessage = context.Message;

            await _userService.ResetPasswordAsync(nessage.Username);
        }
    }
}