using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper
{
    public interface IRepository<T> where T : class
    {
        #region
        List<T> GetList(string sql);

        int GetByVersion(string sql, T entity);

        T GetByVersion(T entity);

        dynamic Insert(T entity);

        dynamic Execute(string sql);

        dynamic Execute(string sql, T entity);

        bool Update(T entity);

        void UpdateState(T entity);

        bool Delete(T entity);
        #endregion
    }
}
