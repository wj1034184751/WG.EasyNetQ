using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper
{
    public interface IETRepository<T> : IRepository<T> where T : class
    {
    }

    public class ETRepository<T> : Repository<T, ETDbContext>, IETRepository<T> where T : class
    {
        public ETRepository(ETDbContext context) : base(context)
        {

        }
    }
}
