using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace qsLibPack.Repositories.EF
{
    public abstract class QueryRepository
    {
        readonly DbContext _context;
        protected QueryRepository(DbContext context)
        {
            _context = context;
        }

        protected IQueryable<Tdto> Select<Tdto>(string sql) where Tdto: class
        {
            return _context.Set<Tdto>().FromSqlRaw(sql);
        }
    }
}