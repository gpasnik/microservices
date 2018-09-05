using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GP.Microservices.Common.ServiceClients
{
    public class Response<T>
    {
        public Response(T result, IDictionary<string,object> errors = null)
        {
            Result = result;
            Errors = errors;
        }

        public T Result { get; }
        public IDictionary<string, object> Errors { get; }
        public bool Success => !Failure;
        public bool Failure => Errors?.Any() == true;
        public string AggregatedErrors => Errors?
            .Values
            .Take(3)
            .Where(x => x != null)
            .Select(x => x.ToString())
            .Aggregate((a, b) => $"{a} {b}");

        public static async Task<Response<T>> CreateAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode == false)
            {
                var error = JsonConvert.DeserializeObject<ServiceError>(content);

                return new Response<T>(default(T), new Dictionary<string, object>
                {
                    {"message", error.Message },
                    {"type", error.Type },
                    {"code", error.Code },
                    {"service", error.Service },
                    {"trace_id", error.TraceId }
                });
            }
            var result = JsonConvert.DeserializeObject<T>(content);

            return new Response<T>(result);
        }
    }
}