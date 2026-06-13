using System.Threading;
using System.Threading.Tasks;
using System;
using qsLibPack.Domain.Entities;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IRepository<T, TId> where T : AggregateRoot<TId>
    {   
        void Create(T entity);
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        T? GetByID(TId id);
        Task<T?> GetByIDAsync(TId id, CancellationToken cancellationToken = default);
        void Remove(T entity);
        Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    }
}
