using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper
{
    public class Repository<T> : BaseDbContext where T : class
    {

        public List<T> GetList(string sql)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<T>(sql);
                return result.ToList<T>();
            }
        }


        public int GetByVersion(string sql, CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<int>(sql, par);
                return result.FirstOrDefault();
            }
        }

        public dynamic Insert(T entity)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Insert<T>(entity);
                return model;
            }
        }


        public dynamic Execute(string sql)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Execute(sql);
                return model;
            }
        }

        public dynamic Execute(string sql, CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Execute(sql, par);
                return model;
            }
        }

        public bool Update(T entity)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Update<T>(entity);
                return model;
            }
        }

        public async Task<bool> Delete(T entity)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Delete<T>(entity);
                return model;
            }
        }
    }
}
