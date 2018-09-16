using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GP.Microservices.Common;
using GP.Microservices.Common.Authentication;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Common.Messages.Users.Events;
using GP.Microservices.Common.Middlewares;
using GP.Microservices.Storage.Domain.Repositories;
using GP.Microservices.Storage.Handlers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace GP.Microservices.Storage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "Storage API", Version = "v1.0" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            services.AddJwtAuthentication(Configuration);

            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<RemarkRepository>().AsImplementedInterfaces();
            builder.RegisterType<UserRepository>().AsImplementedInterfaces();

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

                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserActivated)}", c => c.Consumer<UserEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserBlocked)}", c => c.Consumer<UserEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserDeleted)}", c => c.Consumer<UserEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserRegistered)}", c => c.Consumer<UserEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserUnblocked)}", c => c.Consumer<UserEventHandler>(context));

                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(ActivityAdded)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(CommentAdded)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(CommentRemoved)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(ImageAdded)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(ImageRemoved)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkCanceled)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkCreated)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkDeleted)}", c => c.Consumer<RemarkEventHandler>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkResolved)}", c => c.Consumer<RemarkEventHandler>(context));
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime appLifetime)
        {
            app.UseCors(o => o.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Storage API");
                c.DocExpansion(DocExpansion.None);
            });
            app.UseAuthentication();
            app.UseMvc();

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
