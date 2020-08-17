using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.Ioc
{
    /// <summary>
    /// 注册服务接口
    /// </summary>
    public interface IServiceCollection
    {
        IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class;
    }
}
