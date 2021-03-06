﻿using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GP.Microservices.Common;
using GP.Microservices.Common.Authentication;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.Middlewares;
using GP.Microservices.Users.Data;
using GP.Microservices.Users.Domain.Services;
using GP.Microservices.Users.Handlers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddCors();
            services.AddMvc()
                .AddControllersAsServices();
            services.AddAutofac();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "Users API", Version = "v1.0" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            services.AddJwtAuthentication(Configuration);

            var connection = Configuration.GetConnectionString("UsersDb");
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));

            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterConsumers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UserService>().AsImplementedInterfaces();

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

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(RegisterUser)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
                        });

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(ActivateUser)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
                        });

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(DeleteUser)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
                        });

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(BlockUser)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
                        });

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(UnblockUser)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
                        });

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(ChangeUserPassword)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
                        });

                        cfg.ReceiveEndpoint(host, $"UserService:{nameof(ResetUserPassword)}", c =>
                        {
                            // otherwise, be smart, register explicitly
                            c.Consumer<UserCommandHandler>(context);
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
            app.UseCors(o => o.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Users API");
                c.DocExpansion(DocExpansion.None);
            });
            app.UseAuthentication();
            app.UseMvc();

            var context = ApplicationContainer.Resolve<UsersContext>();
            if (context.Database.EnsureCreated() == false)
            {
                Console.WriteLine("creating db");
            }

            var busControl = ApplicationContainer.Resolve<IBusControl>();
            busControl.Start();

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
