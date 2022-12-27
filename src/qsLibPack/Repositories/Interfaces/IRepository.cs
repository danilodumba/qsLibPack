using System.Threading.Tasks;
using System;
using qsLibPack.Domain.Entities;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IRepository<T, TId> where T : AggregateRoot<TId>
    {   
        void Create(T entity);
        Task CreateAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        T GetByID(TId id);
        Task<T> GetByIDAsync(TId id);
        void Remove(T entity);
        Task RemoveAsync(T entity);
    }
}
