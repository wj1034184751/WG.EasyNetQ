using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.Ioc
{
    public class ServiceCollection : IServiceCollection
    {
        private ContainerBuilder _builder;

        private List<ServiceDescriptor> ListServiceDescriptor;

        public IContainer Container { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public ServiceCollection()
        {
            this._builder = new ContainerBuilder();

            this.ListServiceDescriptor = new List<ServiceDescriptor>();
        }

        public void BeginRegister()
        {
            ServiceProvider = new ServiceProvider();
            this.AddInstance(ServiceProvider);
            Container = this._builder.Build();
            ServiceProvider.Container = Container;
        }

        public IServiceCollection AddInstance<IService>(IService instance) where IService:class
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(instance.GetType(), typeof(IService)));
            _builder.RegisterInstance(instance).As<IService>();
            return this;
        }

        public IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(implementationInstance.GetType(), typeof(TService)));
            _builder.RegisterInstance(implementationInstance).As<TService>().SingleInstance();
            return this;
        }

        public IServiceCollection AddSingleton(Type serviceType, Type implementationType)
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(implementationType, serviceType));
            _builder.RegisterGeneric(implementationType).As(serviceType).SingleInstance();
            return this;
        }

        public IServiceCollection AddSingletonGeneric(Type serviceType, Type implementationType)
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(implementationType, serviceType));
            _builder.RegisterAssemblyTypes(serviceType.GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(implementationType))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            return this;
        }

        public IServiceCollection AddSingletonGeneric(Type serviceType)
        {
            foreach(var item in serviceType.GetTypeInfo().Assembly.GetTypes().Where(t=>t.IsGenericType&&!t.IsInterface))
            {
                _builder.RegisterGeneric(item).AsImplementedInterfaces().InstancePerLifetimeScope();
            }
            return this;
        }

        public IServiceCollection AddScoped<IService, TImplementation>()
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(IService)));
            _builder.RegisterType<TImplementation>().As<IService>().InstancePerLifetimeScope();// 构造函数注入
            return this;
        }

        public IServiceCollection AddScoped<IService>(Func<IComponentContext, string> func)
        {
            //ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(IService)));
            _builder.Register(func).InstancePerLifetimeScope();
            return this;
        }
    }
}
