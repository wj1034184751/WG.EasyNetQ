using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.Ioc
{
    public interface IServiceProvider
    {
        T GetRequiredService<T>();
    }
}
