using App.Metrics.Counter;
using App.Metrics.Timer;

namespace GP.Microservices.Api.Metrics
{
    public interface IMetricsOptionsFactory
    {
        CounterOptions GetCounterOptions(string name);
    }
}