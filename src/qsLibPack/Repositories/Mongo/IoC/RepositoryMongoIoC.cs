using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Repositories.Mongo.Contexts;
using qsLibPack.Repositories.Mongo.Core;

namespace qsLibPack.Repositories.Mongo.IoC
{
    public static class RepositoryMongoIoC
    {
        public static void AddQsLibPackMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection("MongoConnection"));
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}