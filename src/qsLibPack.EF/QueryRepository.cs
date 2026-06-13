using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace qsLibPack.Repositories.EF
{
    public abstract class QueryRepository
    {
        protected readonly DbContext _context;
        protected QueryRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Faz o select em seu banco de dados usando o LINQ do Entity Framework (EF)
        /// </summary>
        /// <typeparam name="TEntity">Sua classe de entidade</typeparam>
        protected IQueryable<TEntity> SelectLinq<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// Faz o select em seu banco de dados
        /// </summary>
        /// <typeparam name="Tdto"></typeparam>
        protected IList<Tdto> SelectSql<Tdto>(string sql, params object[] parameters) where Tdto : class
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            _context.Database.OpenConnection();
            try
            {
                using var result = command.ExecuteReader();
                return DataReaderMapToList<Tdto>(result);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Executa queries de retorno unico. Ex select count from table
        /// </summary>
        protected object ExecuteScalar(string sql, params object[] parameters)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            _context.Database.OpenConnection();
            try
            {
                return command.ExecuteScalar();
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        private static List<Tdto> DataReaderMapToList<Tdto>(DbDataReader dr)
        {
            var list = new List<Tdto>();

            if (!dr.HasRows)
                return list;

            var columns = new HashSet<string>(dr.GetColumnSchema().Select(x => x.ColumnName.ToUpperInvariant()), StringComparer.OrdinalIgnoreCase);
            var props = typeof(Tdto).GetProperties();

            while (dr.Read())
            {
                var dto = Activator.CreateInstance<Tdto>();
                foreach (PropertyInfo prop in props)
                {
                    if (columns.Contains(prop.Name.ToUpperInvariant()) && !Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(dto, dr[prop.Name], null);
                    }
                }
                list.Add(dto);
            }

            return list;
        }
    }
}
