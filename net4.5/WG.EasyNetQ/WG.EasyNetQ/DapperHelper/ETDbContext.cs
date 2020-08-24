using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Uti;

namespace WG.EasyNetQ.DapperHelper
{
    public class ETDbContext
    {
        private IDbConnection _dbConnection;
        protected string _connectionString = string.Empty;
        public IDbConnection DbConnection
        {
            get
            {
                _dbConnection = new SqlConnection(_connectionString);
                return _dbConnection;
            }
        }

        public ETDbContext(string connectionString)
        {
            this._connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbTransaction DbTransaction { get; set; }

        /// <summary>
        /// 是否已被提交
        /// </summary>
        public bool Committed { get; private set; }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            Committed = false;
            bool isClosed = DbConnection.State == ConnectionState.Closed;
            if (isClosed) DbConnection.Open();
            DbTransaction = DbConnection != null ? DbConnection.BeginTransaction() : null;
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void CommitTransaction()
        {
            DbTransaction.Commit();
            Committed = true;

            Dispose();
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollBackTransaction()
        {
            DbTransaction.Rollback();
            Committed = true;

            Dispose();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DbTransaction.Dispose();
            if (DbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }
    }
}
