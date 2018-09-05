using System;
using System.Threading.Tasks;

namespace GP.Microservices.Common.ServiceClients
{
    public static class ResponseExtensions
    {
        public static async Task<T> OrFailAsync<T>(this Task<Response<T>> response)
        {
            var result = await response;
            if (result.Failure)
                throw new Exception(result.AggregatedErrors);

            return result.Result;
        }

        public static async Task<T> OrFailAsync<T>(this Task<Response<T>> response, Func<Response<T>, Task> handler)
        {
            var result = await response;
            if (result.Failure)
                await handler(result);

            return result.Result;
        }

        public static async Task<T> OrFailAsync<T>(this Task<Response<T>> response, Action<Response<T>> handler)
        {
            var result = await response;
            if (result.Failure)
                handler(result);

            return result.Result;
        }
    }
}