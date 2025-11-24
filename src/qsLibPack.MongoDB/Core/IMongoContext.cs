using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace qsLibPack.Repositories.Mongo.Core
{
    public interface IMongoContext : IDisposable
    {
        void AddCommand(Func<CancellationToken, Task> func);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IMongoCollection<T> GetCollection<T>(string name);
    }
}