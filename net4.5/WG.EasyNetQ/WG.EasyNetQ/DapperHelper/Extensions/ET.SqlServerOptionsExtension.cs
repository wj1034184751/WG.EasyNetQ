using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core.Ioc;
using WG.EasyNetQ.DapperHelper.Core;
using WG.EasyNetQ.ETCore;
using WG.EasyNetQ.Extensions;

namespace WG.EasyNetQ.DapperHelper.Extensions
{
    internal class SqlServerETOptionsExtension : IETOptionsExtension
    {
        private readonly Action<SqlServerOptions> _configure;

        public SqlServerETOptionsExtension(Action<SqlServerOptions> configure)
        {
            this._configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingletonGeneric(typeof(Repository<,>));
            services.AddScoped<IETPublisher, ETPublisher>();
            AddSqlServerOptions(services);
        }

        private void AddSqlServerOptions(IServiceCollection services)
        {
            var sqlServerOptions = new SqlServerOptions();

            _configure(sqlServerOptions);

            services.AddSingleton(new ETDbContext(sqlServerOptions.ConnectionString));
        }
    }
}
