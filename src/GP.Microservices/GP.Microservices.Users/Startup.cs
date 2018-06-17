using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GP.Microservices.Common;
using GP.Microservices.Users.Data;
using GP.Microservices.Users.Handlers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GP.Microservices.Users
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

            var connection = Configuration.GetConnectionString("UsersDb");
            //var connection = @"Server=db;Database=GP.Microservices.Users;User=sa;Password=123QWEasd;MultipleActiveResultSets=true;";
            //var connection = @"Server=host.docker.internal,52915;Database=GP.Microservices.Users;User=sa;Password=123QWEasd;MultipleActiveResultSets=true;";
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));

            // Create the container builder.
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

                        cfg.ReceiveEndpoint(host, "sample_message_queue", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<SampleMessageHandler>(context);
                        });
                    });

                    return busControl;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            ApplicationContainer = builder.Build();

            var bc = ApplicationContainer.Resolve<IBusControl>();
            bc.Start();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            var context = ApplicationContainer.Resolve<UsersContext>();
            if (context.Database.EnsureCreated() == false)
            {
                Console.WriteLine("creating db");
            }

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
