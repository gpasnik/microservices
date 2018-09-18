using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

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

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static async Task<Response<T>> CreateAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode == false)
            {
                var error = string.IsNullOrWhiteSpace(content)
                 ? new ServiceError(response.ReasonPhrase, (int) response.StatusCode)
                 : JsonConvert.DeserializeObject<ServiceError>(content);

                Logger.Error($"Can't get response from {response.RequestMessage.RequestUri}, " +
                              $"reasonPhrase {response.ReasonPhrase}, statusCode: {response.StatusCode}, content: {content}");

                return new Response<T>(default(T), error);
            }
            var result = JsonConvert.DeserializeObject<T>(content);

            return new Response<T>(result);
        }
    }
}