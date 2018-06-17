using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GP.Microservices.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc();
            services.AddAutofac();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());
            builder.Register(context =>
                {
                    var config = new RabbitMqConfiguration();
                    Configuration.Bind("RabbitMq", config);
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

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
