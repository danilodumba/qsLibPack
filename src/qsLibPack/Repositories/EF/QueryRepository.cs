using System.Collections;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using qsLibPack.Domain.Entities;
using System.Collections.Generic;

namespace qsLibPack.Repositories.EF
{
    public abstract class QueryRepository
    {
        readonly DbContext _context;
        protected QueryRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Faz o select em seu banco de dados
        /// </summary>
        /// <typeparam name="Tdto">Sua classe de DTO</typeparam>
        protected IQueryable<Tdto> SelectSql<Tdto>(string sql) where Tdto: class
        {
            return _context.Set<Tdto>().FromSqlRaw(sql);
        }

        /// <summary>
        /// Faz o select em seu banco de dados
        /// </summary>
        /// <typeparam name="Tdto"></typeparam>
        protected IQueryable<Tdto> SelectSql<Tdto>(string sql, params object[] parameters) where Tdto: class
        {
            return _context.Set<Tdto>().FromSqlRaw(sql, parameters);
        }

        /// <summary>
        /// Faz o select em seu banco de dados usando o Entity Framework (EF)
        /// </summary>
        /// <typeparam name="TEntity">Sua classe de entidade</typeparam>
        protected IEnumerable<TEntity> Where<TEntity>(Func<TEntity, bool> predicate) where TEntity: class
        {
            return _context.Set<TEntity>().Where(predicate);
        }
    }
}