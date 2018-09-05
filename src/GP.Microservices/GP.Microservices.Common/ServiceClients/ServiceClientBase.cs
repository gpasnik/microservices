using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GP.Microservices.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GP.Microservices.Common.ServiceClients
{
    public abstract class ServiceClientBase
    {
        protected readonly HttpClient HttpClient;
        protected readonly ServiceClientConfiguration Configuration;

        protected ServiceClientBase(HttpClient httpClient, IConfiguration config)
        {
            HttpClient = httpClient;
            var sectionName = GetType().Name;

            HttpClient = httpClient;
            var configuration = new ServiceClientConfiguration();
            config.GetSection(sectionName).Bind(configuration);
            Configuration = configuration;

            HttpClient.BaseAddress = new Uri(Configuration.ApiUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<Response<T>> GetAsync<T>(string endpoint)
        {
            var response = await HttpClient
                .GetAsync(WebUtility.UrlEncode(endpoint))
                .ConfigureAwait(false);

            return await Response<T>.CreateAsync(response);
        }

        protected async Task<Response<T>> PostAsync<T>(string endpoint, object payload)
        {
            var requestContent = GetStringPayload(payload);

            var response = await HttpClient
                .PostAsync(WebUtility.UrlEncode(endpoint), requestContent);

            return await Response<T>.CreateAsync(response);
        }

        protected async Task<Response<T>> PutAsync<T>(string endpoint, object payload)
        {
            var requestContent = GetStringPayload(payload);

            var response = await HttpClient
                .PutAsync(WebUtility.UrlEncode(endpoint), requestContent);

            return await Response<T>.CreateAsync(response);
        }

        protected async Task<Response<T>> DeleteAsync<T>(string endpoint)
        {
            var response = await HttpClient
                .DeleteAsync(WebUtility.UrlEncode(endpoint));

            return await Response<T>.CreateAsync(response);
        }

        private StringContent GetStringPayload<T>(T payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}