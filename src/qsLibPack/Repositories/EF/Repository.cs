using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using qsLibPack.Domain.Entities;

namespace qsLibPack.Repositories.EF
{
    public abstract class Repository<T, TId> where T : AggregateRoot<TId>
    {
        protected DbSet<T> _dbSet;

        protected Repository(DbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public virtual void Create(T entity)
        {
            entity.Validate();
            _dbSet.Add(entity);
        }

        public async virtual Task CreateAsync(T entity)
        {
            entity.Validate();
            await _dbSet.AddAsync(entity);
        }
        public virtual void Update(T entity)
        {
            entity.Validate();
            _dbSet.Update(entity);
        }

        public Task UpdateAsync(T entity)
        {
            entity.Validate();
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual T GetByID(TId id)
        {
            return _dbSet.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async virtual Task<T> GetByIDAsync(TId id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}