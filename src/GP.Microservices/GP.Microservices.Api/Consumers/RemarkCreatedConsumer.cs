using System;
using System.Threading.Tasks;
using App.Metrics;
using GP.Microservices.Api.Metrics;
using GP.Microservices.Common.Messages.Remarks.Events;
using MassTransit;

namespace GP.Microservices.Api.Consumers
{
    public class RemarkCreatedConsumer : IConsumer<RemarkCreated>
    {
        private readonly IMetrics _metrics;
        private readonly IMetricsOptionsFactory _factory;

        public RemarkCreatedConsumer(IMetrics metrics,
            IMetricsOptionsFactory factory)
        {
            _metrics = metrics;
            _factory = factory;
        }

        public async Task Consume(ConsumeContext<RemarkCreated> context)
        {
            var options = _factory.GetCounterOptions(typeof(RemarkCreated).Name);
            _metrics.Measure.Counter.Increment(options);
        }
    }
}