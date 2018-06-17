using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GP.Microservices.Common;
using GP.Microservices.Users.Data;
using MassTransit;

namespace GP.Microservices.Users.Handlers
{
    public class SampleMessageHandler : IConsumer<SampleMessage>
    {
        private readonly UsersContext _usersContext;

        public SampleMessageHandler(UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        public async Task Consume(ConsumeContext<SampleMessage> context)
        {
            var sw = new Stopwatch();
            sw.Start();

            var message = new Message
            {
                Id = Guid.NewGuid(),
                Text = context.Message.Message
            };

            await _usersContext
                .Messages
                .AddAsync(message);

            await _usersContext.SaveChangesAsync();

            sw.Stop();
            await context.NotifyConsumed(context, sw.Elapsed, nameof(SampleMessageHandler));
        }
    }
}