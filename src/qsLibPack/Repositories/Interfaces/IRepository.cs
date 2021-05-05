using System.Threading.Tasks;
using System;
using qsLibPack.Domain.Entities;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IRepository<T, TId> where T : AggregateRoot<TId>
    {   
        [ObsoleteAttribute("This method is obsolete. Call CreateAsync instead.", false)]
        void Create(T entity);
        Task CreateAsync(T entity);

        [ObsoleteAttribute("This method is obsolete. Call UpdateAsync instead.", false)]
        void Update(T entity);
        Task UpdateAsync(T entity);
        
        [ObsoleteAttribute("This method is obsolete. Call GetByIDAsync instead.", false)]
        T GetByID(TId id);
        Task<T> GetByIDAsync(TId id);

        [ObsoleteAttribute("This method is obsolete. Call RemoveAsync instead.", false)]
        void Remove(T entity);
        Task RemoveAsync(T entity);
    }
}
