using System;
using System.Collections.Generic;
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
            Func<Task> func = async () => await _dbSet.InsertOneAsync(entity);
            _context.AddCommand(func);
        }

        public virtual Task CreateAsync(TEntity entity)
        {
            Func<Task> func = async () => await _dbSet.InsertOneAsync(entity);
            _context.AddCommand(func);

            return Task.CompletedTask;
        }

        public virtual TEntity GetByID(TId id)
        {
            var data = _dbSet.Find(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }

        public async virtual Task<TEntity> GetByIDAsync(TId id)
        {
            var data = await _dbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }

        public virtual void Remove(TEntity entity)
        {
           _context.AddCommand(() => _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));
        }

        public virtual void Update(TEntity entity)
        {
            _context.AddCommand(async () =>
            {
                await _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity);
            });
        }

        public virtual async Task<IEnumerable<TEntity>> ListAll()
        {
            var all = await _dbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.AddCommand(async () =>
            {
                await _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity);
            });

            return Task.CompletedTask;
        }

        public Task RemoveAsync(TEntity entity)
        {
           _context.AddCommand(() => _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));
           return Task.CompletedTask;
        }
    }
}