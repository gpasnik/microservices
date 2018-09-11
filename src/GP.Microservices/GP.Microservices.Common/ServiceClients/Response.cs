using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GP.Microservices.Common.ServiceClients
{
    public class Response<T>
    {
        public Response(T result, ServiceError error = null)
        {
            Result = result;
            Error = error;
        }

        public T Result { get; }
        public ServiceError Error { get; }
        public bool Success => !Failure;
        public bool Failure => Error != null;
        public string AggregatedErrors => $"{Error?.Message} {Error?.Code}";

        public static async Task<Response<T>> CreateAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode == false)
            {
                var error = JsonConvert.DeserializeObject<ServiceError>(content);

                return new Response<T>(default(T), error);
            }
            var result = JsonConvert.DeserializeObject<T>(content);

            return new Response<T>(result);
        }
    }
}