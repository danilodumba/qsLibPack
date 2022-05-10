using System;
using System.Threading.Tasks;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        [ObsoleteAttribute("This method is obsolete. Call CommitAsync instead.", false)]
        void Commit();
        Task CommitAsync();
    }
}