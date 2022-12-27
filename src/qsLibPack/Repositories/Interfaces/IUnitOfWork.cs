using System;
using System.Threading.Tasks;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}