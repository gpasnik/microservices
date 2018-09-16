using Autofac;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GP.Microservices.Storage.Mongo
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                var settings = c.Resolve<IOptions<MongoDbSettings>>();

                return new MongoClient(settings.Value.ConnectionString);
            }).SingleInstance();

            builder.Register((c, p) =>
            {
                var mongoClient = c.Resolve<MongoClient>();
                var settings = c.Resolve<IOptions<MongoDbSettings>>();
                var database = mongoClient.GetDatabase(settings.Value.Database);

                return database;
            }).As<IMongoDatabase>();
        }
    }
}