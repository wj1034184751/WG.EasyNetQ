using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core;
using WG.EasyNetQ.DapperHelper.Core;

namespace WG.EasyNetQ.DapperHelper.Extensions
{
    public static class ETOptionsExtensions
    {
        public static ETOptions UseSqlServer(this ETOptions options, string connectionString)
        {
            return null;
        }

        public static ETOptions UseSqlServer(this ETOptions options,Action<SqlServerOptions> configure)
        {
            if(configure==null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return options;
        }
    }
}
