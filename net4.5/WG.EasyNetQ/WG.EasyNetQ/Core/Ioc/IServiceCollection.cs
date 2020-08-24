using Autofac;
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
        IServiceProvider ServiceProvider { get; set; }

        IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class;

        IServiceCollection AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
     
        IServiceCollection AddSingleton(Type serviceType, Type implementationType);

        IServiceCollection AddSingletonGeneric(Type serviceType, Type implementationType);

        IServiceCollection AddSingletonGeneric(Type serviceType);

        IServiceCollection AddScoped<IService, ITmplementation>();

        IServiceCollection AddScoped<IService>(Func<IComponentContext, string> func);

        void BeginRegister();
    }
}
