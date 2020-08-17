using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core.Ioc;

namespace WG.EasyNetQ.Core
{
    public sealed class ETBuilder
    {
        public IServiceCollection Services { get; set; }

        public ETBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

    }
}
