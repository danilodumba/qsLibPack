using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<Func<Task>> _commands;
        private readonly MongoSettings _settings;

        public MongoContext(IOptions<MongoSettings> settings)
        {
            _settings = settings.Value;
            _commands = new List<Func<Task>>();
        }
        
        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            Configure();
            return Database.GetCollection<T>(name);
        }

        public async Task<int> SaveChangesAsync()
        {
            Configure();

            using (Session = await MongoClient.StartSessionAsync())
            {
                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);
            }

            _commands.Clear();
            return _commands.Count;
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