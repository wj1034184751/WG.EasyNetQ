using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper
{
    public class DapperSqlHelper
    {
        private static IDbConnection _dbConnection;
        protected string _connectionString = string.Empty;
        public static IDbConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ToString());
                    _dbConnection.Open();
                }

                if (string.IsNullOrWhiteSpace(_dbConnection.ConnectionString))
                {
                    _dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Dbconn"].ToString();
                }
                return _dbConnection;
            }
        }

        public static dynamic Execute(string sql)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Execute(sql);
                return model;
            }
        }

        public static dynamic Execute(string sql, object par)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Execute(sql, par);
                return model;
            }
        }

        public static dynamic Insert<T>(T entity) where T : class
        {
            using (var connection = DbConnection)
            {
                var model = connection.Insert<T>(entity);
                return model;
            }
        }

        public static dynamic Update<T>(T entity) where T : class
        {
            using (var connection = DbConnection)
            {
                var model = connection.Update<T>(entity);
                return model;
            }
        }

        public static CustomerQueue GetByVersion(string sql, CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<CustomerQueue>(sql, par);
                return result.FirstOrDefault();
            }
        }

        public static int GetCountByVersion(string sql, CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<int>(sql, par);
                return result.FirstOrDefault();
            }
        }

        public static dynamic Execute(string sql, CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var model = connection.Execute(sql, par);
                return model;
            }
        }

        public static List<CustomerQueue> GetList(string sql)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<CustomerQueue>(sql);
                return result.ToList<CustomerQueue>();
            }
        }
    }
}
