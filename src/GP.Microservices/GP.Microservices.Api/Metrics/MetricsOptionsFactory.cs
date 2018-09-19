using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Timer;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace GP.Microservices.Api.Metrics
{
    public class MetricsOptionsFactory : IMetricsOptionsFactory
    {
        private readonly string _context;

        public MetricsOptionsFactory(IConfiguration configuration)
        {
            _context = configuration.GetSection("MetricsOptions").GetValue<string>("DefaultContextLabel");
        }

        public CounterOptions GetCounterOptions(string name)
        {
            return new CounterOptions
            {
                Context = _context,
                MeasurementUnit = Unit.Calls,
                Name = name,
            };
        }
    }
}