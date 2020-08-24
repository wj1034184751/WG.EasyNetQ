using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core;
using WG.EasyNetQ.DapperHelper.Core;
using WG.EasyNetQ.DapperHelper.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ETSqlOptionsExtensions
    {
        public static ETOptions UseSqlServer(this ETOptions options, string connectionString)
        {
            return options.UseSqlServer(opt => { opt.ConnectionString = connectionString; });
        }

        public static ETOptions UseSqlServer(this ETOptions options,Action<SqlServerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new SqlServerETOptionsExtension(configure));
            return options;
        }
    }
}
