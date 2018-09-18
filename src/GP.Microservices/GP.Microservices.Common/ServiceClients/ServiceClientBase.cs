using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GP.Microservices.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;

namespace GP.Microservices.Common.ServiceClients
{
    public abstract class ServiceClientBase
    {
        protected readonly HttpClient HttpClient;
        protected readonly ServiceClientConfiguration Configuration;
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected ServiceClientBase(HttpClient httpClient, IConfiguration config)
        {
            HttpClient = httpClient;
            var sectionName = GetType().Name;

            HttpClient = httpClient;
            var configuration = new ServiceClientConfiguration();
            config.GetSection(sectionName).Bind(configuration);
            Configuration = configuration;

            Logger.Trace($"Creating HttpClient for {sectionName} with BaseAddress: {Configuration.ApiUrl}");

            HttpClient.BaseAddress = new Uri(Configuration.ApiUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Logger.Trace($"HttpClient created for {sectionName} with BaseAddress: {HttpClient.BaseAddress}");
        }

        protected async Task<Response<T>> GetAsync<T>(string endpoint)
        {
            Logger.Trace($"Performing GET Request to: {HttpClient.BaseAddress}/{endpoint}");

            var response = await HttpClient
                .GetAsync(UrlEncodeEndpoint(endpoint))
                .ConfigureAwait(false);

            Logger.Trace($"GET Response StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");

            return await Response<T>.CreateAsync(response);
        }

        protected async Task<Response<T>> PostAsync<T>(string endpoint, object payload)
        {
            Logger.Trace($"Performing POST Request to: {HttpClient.BaseAddress}/{endpoint}");

            var requestContent = GetStringPayload(payload);

            var response = await HttpClient
                .PostAsync(UrlEncodeEndpoint(endpoint), requestContent);

            Logger.Trace($"POST Response StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");

            return await Response<T>.CreateAsync(response);
        }

        protected async Task<Response<T>> PutAsync<T>(string endpoint, object payload)
        {
            Logger.Trace($"Performing PUT Request to: {HttpClient.BaseAddress}/{endpoint}");

            var requestContent = GetStringPayload(payload);

            var response = await HttpClient
                .PutAsync(UrlEncodeEndpoint(endpoint), requestContent);

            Logger.Trace($"PUT Response StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");

            return await Response<T>.CreateAsync(response);
        }

        protected async Task<Response<T>> DeleteAsync<T>(string endpoint)
        {
            Logger.Trace($"Performing DELETE Request to: {HttpClient.BaseAddress}/{endpoint}");

            var response = await HttpClient
                .DeleteAsync(UrlEncodeEndpoint(endpoint));

            Logger.Trace($"DELETE Response StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");

            return await Response<T>.CreateAsync(response);
        }

        private string UrlEncodeEndpoint(string endpoint)
        {
            var segments = endpoint.Split('/')
                .Select(WebUtility.UrlEncode)
                .ToList();

            return string.Join('/', segments);
        }

        private StringContent GetStringPayload<T>(T payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}