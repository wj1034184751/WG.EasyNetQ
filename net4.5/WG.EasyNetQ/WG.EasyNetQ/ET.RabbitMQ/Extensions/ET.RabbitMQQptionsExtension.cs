using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core.Ioc;
using WG.EasyNetQ.ET.RabbitMQ.Core;
using WG.EasyNetQ.Extensions;

namespace WG.EasyNetQ.ET.RabbitMQ.Extensions
{
    internal sealed class RabbitMQETOptionsExtension : IETOptionsExtension
    {
        private readonly Action<RabbitMQOptions> _configure;

        public RabbitMQETOptionsExtension(Action<RabbitMQOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new RabbitMQOptions();
            _configure?.Invoke(options);
            services.AddSingleton(options);
            services.AddSingleton<IRabbitMQClient,RabbitMQClient>();
        }
    }
}
