using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using qsLibPack.Repositories.Mongo.Core;

namespace qsLibPack.Repositories.Mongo.Contexts
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<CancellationToken, Task>> _commands;
        private readonly MongoSettings _settings;

        public MongoContext(IOptions<MongoSettings> settings)
        {
            _settings = settings.Value;
            _commands = new List<Func<CancellationToken, Task>>();
        }
        
        public void AddCommand(Func<CancellationToken, Task> func)
        {
            _commands.Add(func);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            Configure();
            return Database.GetCollection<T>(name);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Configure();

            using (Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken))
            {
                var commandTasks = _commands.Select(c => c(cancellationToken));
                await Task.WhenAll(commandTasks);
            }

            var executedCount = _commands.Count;
            _commands.Clear();
            return executedCount;
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        private void Configure()
        {
            if (MongoClient != null)
            {
                return;
            }

            MongoClient = new MongoClient(_settings.ConnectionString);
            Database = MongoClient.GetDatabase(_settings.Database);
        }    
    }
}