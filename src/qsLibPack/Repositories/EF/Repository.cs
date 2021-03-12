using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using qsLibPack.Domain.Entities;

namespace qsLibPack.Repositories.EF
{
    public abstract class Repository<T, TId> where T : AggregateRoot<TId>
    {
        protected DbContext _context;
        protected DbSet<T> _dbSet;

        protected Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public async virtual Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public virtual T GetByID(TId id)
        {
            return _dbSet.FirstOrDefault(x => x.Id.Equals(id));
        }
        public async virtual Task<T> GetByIDAsync(TId id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}