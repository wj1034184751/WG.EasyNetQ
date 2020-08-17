using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper.Core
{
    public class SqlServerOptions : EFOptions
    {
        public string ConnectionString { get; set; }
    }
}
