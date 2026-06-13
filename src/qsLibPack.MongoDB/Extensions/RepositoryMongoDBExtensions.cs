using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Repositories.Mongo.Contexts;
using qsLibPack.Repositories.Mongo.Core;

namespace qsLibPack.Repositories.Mongo.Extensions
{
    public static class RepositoryMongoDBExtensions
    {
        public static void AddQsLibPackMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection("MongoConnection"));
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
