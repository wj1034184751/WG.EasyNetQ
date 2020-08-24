using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.DI
{
    public static class DI
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
