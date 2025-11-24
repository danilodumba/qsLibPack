using System.Threading;
using System.Threading.Tasks;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Repositories.Mongo.Core;

namespace qsLibPack.Repositories.Mongo
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly IMongoContext _context;
        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}