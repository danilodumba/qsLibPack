using System.Threading.Tasks;

namespace qsLibPack.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }
}