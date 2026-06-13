using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using qsLibPack.Repositories.Mongo.Core;

namespace qsLibPack.Repositories.Mongo.Contexts
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly List<Func<CancellationToken, Task>> _commands;

        public MongoContext(IMongoClient mongoClient, IOptions<MongoSettings> settings)
        {
            _database = mongoClient.GetDatabase(settings.Value.Database);
            _commands = new List<Func<CancellationToken, Task>>();
        }

        public void AddCommand(Func<CancellationToken, Task> func)
        {
            _commands.Add(func);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var count = _commands.Count;
            try
            {
                foreach (var command in _commands)
                    await command(cancellationToken).ConfigureAwait(false);
                return count;
            }
            finally
            {
                _commands.Clear();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
