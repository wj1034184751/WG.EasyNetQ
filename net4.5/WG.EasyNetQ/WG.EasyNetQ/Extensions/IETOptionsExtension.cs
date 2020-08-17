using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core.Ioc;

namespace WG.EasyNetQ.Extensions
{
    public interface IETOptionsExtension
    {
        void AddServices(IServiceCollection services);
    }
}
