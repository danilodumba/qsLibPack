using System.Collections;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using qsLibPack.Domain.Entities;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

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
        protected IList<Tdto> SelectSql<Tdto>(string sql) where Tdto: class
        {
            return this.SelectFromSqlADO<Tdto>(sql);
        }

        /// <summary>
        /// Faz o select em seu banco de dados
        /// </summary>
        /// <typeparam name="Tdto"></typeparam>
        protected IList<Tdto> SelectSql<Tdto>(string sql, params object[] parameters) where Tdto: class
        {
            return this.SelectFromSqlADO<Tdto>(sql, parameters);
        }

        private IList<Tdto> SelectFromSqlADO<Tdto>(string sql, params object[] parameters) where Tdto: class
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            _context.Database.OpenConnection();
            using var result = command.ExecuteReader();
            return DataReaderMapToList<Tdto>(result);
        }

        private static List<T> DataReaderMapToList<T>(DbDataReader dr)
        {
            List<T> list = new List<T>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (dr.GetColumnSchema().Any(x => x.ColumnName.ToUpper() == prop.Name.ToUpper()))
                        {
                            if (!Equals(dr[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, dr[prop.Name], null);
                            }
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            return new List<T>();
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