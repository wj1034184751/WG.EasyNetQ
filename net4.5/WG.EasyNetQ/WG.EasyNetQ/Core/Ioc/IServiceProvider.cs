using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.Ioc
{
    public interface IServiceProvider
    {
        IContainer Container { get; set; }

        T GetRequiredService<T>();
    }
}
