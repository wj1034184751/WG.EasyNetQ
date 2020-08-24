using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper
{
    public interface IETRepository<T> : IRepository<T> where T : class
    {
        int GetCountByVersion(T entity);
    }

    public class ETRepository<T> : Repository<T, ETDbContext>, IETRepository<T> where T : class
    {
        public ETRepository(ETDbContext context) : base(context)
        {

        }

        public  int GetCountByVersion(T entity)
        {
            using (var connection = _context.DbConnection)
            {
                var result = connection.Query<int>("select Count(Id) from  [CustomerQueue] where Version=@Version", entity);
                return result.FirstOrDefault();
            }
        }
    }
}
