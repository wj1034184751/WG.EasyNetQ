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
        private static object _obj = new object();
        private static IDbConnection _dbConnection;
        protected string _connectionString = string.Empty;
        public static IDbConnection DbConnection
        {
            get
            {
                _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconn"].ToString());
                //if (_dbConnection == null)
                //{

                //}

                //if (_dbConnection.State == ConnectionState.Closed && string.IsNullOrWhiteSpace(_dbConnection.ConnectionString))
                //{
                //    _dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Dbconn"].ToString();
                //}
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

        public static CustomerQueue GetByVersion(CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<CustomerQueue>("select * from [CustomerQueue] WHERE [Version]=@Version", par);
                return result.FirstOrDefault();
            }
        }

        public static int GetCountByVersion( CustomerQueue par)
        {
            using (var connection = DbConnection)
            {
                var result = connection.Query<int>("select Count(Id) from  [CustomerQueue] where Version=@Version", par);
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

        public static void UpdateState(CustomerQueue model)
        {
            using (var connection = DbConnection)
            {
                connection.Execute("Update [CustomerQueue] set [IsConsume]=@IsConsume where Version=@Version", model);
            }
        }

        public static void Update(CustomerQueue model)
        {
            using (var connection = DbConnection)
            {
                connection.Execute(@"UPDATE [CustomerQueue]
                                       SET[QueueName] =@QueueName
                                          ,[Version] =@Version
                                          ,[QueueValue] =@QueueValue
                                          ,[IsConsume] =@IsConsume
                                          ,[UpdateTime] =@UpdateTime 
                                          ,[CetryCount] =@CetryCount WHERE Version=@Version", model);
            }
        }
    }
}
