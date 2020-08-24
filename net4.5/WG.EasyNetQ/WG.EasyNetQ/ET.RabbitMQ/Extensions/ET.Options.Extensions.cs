using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core;
using WG.EasyNetQ.ET.RabbitMQ.Core;
using WG.EasyNetQ.ET.RabbitMQ.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ETMqOptionsExtensions
    {
        public static ETOptions UseRabbitMQ(this ETOptions options,string hostName)
        {
            return options.UseRabbitMQ(opt => { opt.HostName = hostName; });
        }

        public static ETOptions UseRabbitMQ(this ETOptions options,Action<RabbitMQOptions> configure)
        {
            if(configure==null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new RabbitMQETOptionsExtension(configure));
            return options;
        }
    }
}
