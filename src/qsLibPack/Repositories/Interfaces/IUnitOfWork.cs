using System;
using System.Threading;
using System.Threading.Tasks;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}