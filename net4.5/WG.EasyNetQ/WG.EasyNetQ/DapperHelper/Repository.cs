using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WG.EasyNetQ.DapperHelper
{
    public class Repository<T, IETDbContext> : IRepository<T> where T : class where IETDbContext : ETDbContext
    {
        protected readonly IETDbContext _context;

        public Repository(IETDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region
        public List<T> GetList(string sql)
        {
            using (var connection = _context.DbConnection)
            {
                var result = connection.Query<T>(sql);
                return result.ToList<T>();
            }
        }

        public int GetByVersion(string sql, CustomerQueue par)
        {
            using (var connection = _context.DbConnection)
            {
                var result = connection.Query<int>(sql, par);
                return result.FirstOrDefault();
            }
        }

        public dynamic Insert(T entity)
        {
            using (var connection = _context.DbConnection)
            {
                var model = connection.Insert<T>(entity);
                return model;
            }
        }


        public dynamic Execute(string sql)
        {
            using (var connection = _context.DbConnection)
            {
                var model = connection.Execute(sql);
                return model;
            }
        }

        public dynamic Execute(string sql, CustomerQueue par)
        {
            using (var connection = _context.DbConnection)
            {
                var model = connection.Execute(sql, par);
                return model;
            }
        }

        public bool Update(T entity)
        {
            using (var connection = _context.DbConnection)
            {
                var model = connection.Update<T>(entity);
                return model;
            }
        }

        public void UpdateState(T model)
        {
            using (var connection = _context.DbConnection)
            {
                connection.Execute("Update [CustomerQueue] set [IsConsume]=@IsConsume where Version=@Version", model);
            }
        }

        public bool Delete(T entity)
        {
            using (var connection = _context.DbConnection)
            {
                var model = connection.Delete<T>(entity);
                return model;
            }
        }
        #endregion
    }
}
