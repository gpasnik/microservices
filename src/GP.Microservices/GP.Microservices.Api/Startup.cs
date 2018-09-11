using System;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GP.Microservices.Common;
using GP.Microservices.Common.Authentication;
using GP.Microservices.Common.Middlewares;
using GP.Microservices.Common.ServiceClients;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GP.Microservices.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables("APP_");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc()
                .AddControllersAsServices();
            services.AddAutofac();
            
            services.AddJwtAuthentication(Configuration);

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddHttpClient<IUserServiceClient, UserServiceClient>();
            services.AddHttpClient<IRemarkServiceClient, RemarkServiceClient>();
            services.AddHttpClient<IStorageServiceClient, StorageServiceClient>();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());
            builder.Register(ctx => Configuration).As<IConfiguration>();
            builder.Register(context =>
                {
                    var config = context.Resolve<IOptions<RabbitMqConfiguration>>().Value;
                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri(config.Host), h =>
                        {
                            h.Username(config.User);
                            h.Password(config.Password);
                            h.Heartbeat(5);
                        });
                    });

                    return busControl;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            builder.RegisterType<JwtTokenService>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<UserServiceClient>().AsImplementedInterfaces();
            //builder.RegisterType<RemarkServiceClient>().AsImplementedInterfaces();
            //builder.RegisterType<StorageServiceClient>().AsImplementedInterfaces();

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            app.UseCors(o => o.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseAuthentication();
            app.UseMvc();

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
