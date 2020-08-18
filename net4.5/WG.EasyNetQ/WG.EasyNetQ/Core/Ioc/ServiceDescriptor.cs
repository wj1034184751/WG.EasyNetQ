using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.Ioc
{
    /// <summary>
    /// 注入信息记载
    /// </summary>
    public class ServiceDescriptor
    {
        public Type ImplementationType { get; set; }

        public Type ServiceType { get; set; }

        public ServiceDescriptor(Type implementtationType, Type serviceType)
        {
            ImplementationType = implementtationType;
            ServiceType = serviceType;
        }

        public static ServiceDescriptor Singleton<IService,TImplementation>()
        {
            var instance = new ServiceDescriptor(typeof(TImplementation), typeof(IService));
            return instance;
        }
    }
}
