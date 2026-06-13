using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using qsLibPack.Domain.Entities;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Repositories.Mongo.Core;

namespace qsLibPack.Repositories.Mongo
{
    public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : AggregateRoot<TId>
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> _dbSet;
        protected Repository(IMongoContext context)
        {
            _context = context;
            _dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Create(TEntity entity)
        {
            entity.Validate();
            _context.AddCommand(async ct => await _dbSet.InsertOneAsync(entity, null, ct).ConfigureAwait(false));
        }

        public virtual Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.Validate();
            _context.AddCommand(async ct => await _dbSet.InsertOneAsync(entity, null, ct).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        public virtual TEntity GetByID(TId id)
        {
            var data = _dbSet.Find(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }

        public async virtual Task<TEntity> GetByIDAsync(TId id, CancellationToken cancellationToken = default)
        {
            var data = await _dbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id), cancellationToken: cancellationToken).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        public virtual void Remove(TEntity entity)
        {
            _context.AddCommand(ct => _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), ct));
        }

        public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.AddCommand(ct => _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), ct));
            return Task.CompletedTask;
        }

        public virtual void Update(TEntity entity)
        {
            entity.Validate();
            _context.AddCommand(async ct =>
            {
                await _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity, cancellationToken: ct).ConfigureAwait(false);
            });
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.Validate();
            _context.AddCommand(async ct =>
            {
                await _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity, cancellationToken: ct).ConfigureAwait(false);
            });
            return Task.CompletedTask;
        }

        public virtual async Task<IEnumerable<TEntity>> ListAll(CancellationToken cancellationToken = default)
        {
            var all = await _dbSet.FindAsync(Builders<TEntity>.Filter.Empty, cancellationToken: cancellationToken).ConfigureAwait(false);
            return all.ToList();
        }
    }
}
