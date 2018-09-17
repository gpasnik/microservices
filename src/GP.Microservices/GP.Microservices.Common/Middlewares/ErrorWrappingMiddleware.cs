using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Exceptions;
using GP.Microservices.Common.ServiceClients;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GP.Microservices.Common.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                ServiceError error;
                if (ex is ServiceException serviceException)
                {
                    context.Response.StatusCode = serviceException.ServiceError.HttpStatusCode;
                    error = serviceException.ServiceError;
                }
                else
                {
                    context.Response.StatusCode = 500;
                    error = new ServiceError(ex.Message, 500);
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    var json = JsonConvert.SerializeObject(error);

                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}
