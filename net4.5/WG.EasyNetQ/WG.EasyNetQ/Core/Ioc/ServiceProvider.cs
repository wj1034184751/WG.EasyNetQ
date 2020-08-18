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
        private IContainer _container;

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

        public ILifetimeScope _lifetimeScope;

        public T GetRequiredService<T>()
        {
            throw new NotImplementedException();
        }
    }
}
