using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GP.Microservices.Common;
using GP.Microservices.Common.Authentication;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Common.Messages.Users.Events;
using GP.Microservices.Common.Middlewares;
using GP.Microservices.Common.ServiceClients;
using GP.Microservices.Storage.Domain.Repositories;
using GP.Microservices.Storage.Extensions;
using GP.Microservices.Storage.Handlers;
using GP.Microservices.Storage.Mongo;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;

namespace GP.Microservices.Storage
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
                c.SwaggerDoc("v1.0", new Info { Title = "Storage API", Version = "v1.0" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            services.AddJwtAuthentication(Configuration);

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDb"));

            services.AddHttpClient<IUserServiceClient, UserServiceClient>();
            services.AddHttpClient<IRemarkServiceClient, RemarkServiceClient>();

            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterModule<MongoModule>();

            builder.RegisterType<RemarkRepository>().AsImplementedInterfaces();
            builder.RegisterType<UserRepository>().AsImplementedInterfaces();
            builder.RegisterType<RemarkCategoryRepository>().AsImplementedInterfaces();
            builder.RegisterType<ActivityTypeRepository>().AsImplementedInterfaces();

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

                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserActivated)}", c => c.Consumer<UserActivatedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserBlocked)}", c => c.Consumer<UserBlockedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserDeleted)}", c => c.Consumer<UserDeletedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserRegistered)}", c => c.Consumer<UserRegisteredConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(UserUnblocked)}", c => c.Consumer<UserUnblockedConsumer>(context));

                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(ActivityAdded)}", c => c.Consumer<RemarkActivityAddedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(CommentAdded)}", c => c.Consumer<RemarkCommentAddedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(CommentRemoved)}", c => c.Consumer<RemarkCommentRemovedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(ImageAdded)}", c => c.Consumer<RemarkImageAddedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(ImageRemoved)}", c => c.Consumer<RemarkImageRemovedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkCanceled)}", c => c.Consumer<RemarkCanceledConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkCreated)}", c => c.Consumer<RemarkCreatedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkDeleted)}", c => c.Consumer<RemarkDeletedConsumer>(context));
                        cfg.ReceiveEndpoint(host, $"StorageService:{nameof(RemarkResolved)}", c => c.Consumer<RemarkResolvedConsumer>(context));
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
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env,
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

            var busControl = ApplicationContainer.Resolve<IBusControl>();
            busControl.Start();

            await SeedDatabase();

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }

        private async Task SeedDatabase()
        {
            var database = ApplicationContainer.Resolve<IMongoDatabase>();

            var categoriesCollection = database.GetCollection<RemarkCategoryDto>();
            if (categoriesCollection.CountDocuments(FilterDefinition<RemarkCategoryDto>.Empty) == 0)
            {
                await categoriesCollection.InsertManyAsync(new List<RemarkCategoryDto>
                {
                    new RemarkCategoryDto
                    {
                        Id = Guid.Parse("C5261772-D5FF-4588-AFF2-735A1E5C01B6"),
                        Name = "Damage"
                    },
                    new RemarkCategoryDto
                    {
                        Id = Guid.Parse("B8F48F0F-7E26-4DA4-A28F-38A5C646A08C"),
                        Name = "Issue"
                    },
                    new RemarkCategoryDto
                    {
                        Id = Guid.Parse("276F404B-A545-4A71-BFD8-15BFE32E8F3F"),
                        Name = "Suggestion"
                    },
                    new RemarkCategoryDto
                    {
                        Id = Guid.Parse("ADB13C39-2B03-4FDA-A222-E72C9388C2AC"),
                        Name = "Praise"
                    }
                });
            }

            var activityTypesCollection = database.GetCollection<ActivityTypeDto>();
            if (activityTypesCollection.CountDocuments(FilterDefinition<ActivityTypeDto>.Empty) == 0)
            {
                await activityTypesCollection.InsertManyAsync(new List<ActivityTypeDto>
                {
                    new ActivityTypeDto
                    {
                        Id = Guid.Parse("EB3B4815-0600-4701-BD7B-36852E43699D"),
                        Name = "Cleaning"
                    },
                    new ActivityTypeDto
                    {
                        Id = Guid.Parse("DBF89A9C-E817-4FAB-8B50-37F8E7A02B69"),
                        Name = "Fixing"
                    },
                    new ActivityTypeDto
                    {
                        Id = Guid.Parse("BEFCE9A8-A323-44A9-BC28-AD013CCE5264"),
                        Name = "Activity"
                    }
                });
            }

            var userCollection = database.GetCollection<UserDto>();
            if (userCollection.CountDocuments(FilterDefinition<UserDto>.Empty) == 0)
            {
                await userCollection.InsertManyAsync(new List<UserDto>
                {
                    new UserDto
                    {
                        Id = Guid.Parse("7749874B-DFDD-46E9-AAA8-F2B02CBD19B2"),
                        Username = "user1",
                        Email = "user1-gp@mailinator.com",
                        Name = "User",
                        Lastname = "One",
                        Status = "Active"
                    },
                    new UserDto
                    {
                        Id = Guid.Parse("F3DE0DEF-67D9-49F4-AAC5-E24661941E78"),
                        Username = "user2",
                        Email = "user2-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Two",
                        Status = "Active"
                    },
                    new UserDto
                    {
                        Id = Guid.Parse("96C50DB1-6F47-48EC-A15E-998388FE1C9B"),
                        Username = "user3",
                        Email = "user3-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Three",
                        Status = "Active"
                    },
                    new UserDto
                    {
                        Id = Guid.Parse("B940D0AC-E3BE-4482-B853-EB3FBD7D4E53"),
                        Username = "user4",
                        Email = "user4-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Four",
                        Status = "Active"
                    },
                    new UserDto
                    {
                        Id = Guid.Parse("D5AF270D-58CF-4FDF-BDF3-ACAFA65BA018"),
                        Username = "user5",
                        Email = "user5-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Five",
                        Status = "Active"
                    }
                });
            }
        }
    }
}
