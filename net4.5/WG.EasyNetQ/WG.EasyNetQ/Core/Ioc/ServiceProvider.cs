using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Core.Ioc
{
    public class ServiceProvider : IServiceProvider
    {
        public IServiceCollection ServiceCollection { get; set; }

        private IContainer _container;

        public IContainer Container
        {
            set
            {
                _container = value;
                var scope = _container.BeginLifetimeScope();
                _lifetimeScope = scope;
            }
            get
            {
                return _container;
            }
        }

        public ILifetimeScope _lifetimeScope;

        public ILifetimeScope LifetimeScope
        {
            get
            {
                return _lifetimeScope;
            }
            set
            {
                _lifetimeScope = value;
            }
        }

        public T GetRequiredService<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }
    }
}
