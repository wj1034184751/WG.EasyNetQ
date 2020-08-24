using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.ET.RabbitMQ.Core
{
   public class RabbitMQOptions
    {
        public const string DefaultPass = "guest";

        public const string DefaultUser = "guest";

        public const string DefaultVHost = "/";

        public const string DefaultExchangeName = "et.default.router";

        public const string ExchangeType = "topic";

        public string HostName { get; set; } = "localhost";

        public string Password { get; set; } = DefaultPass;

        public string UserName { get; set; } = DefaultUser;

        public string VirtualHost { get; set; } = DefaultVHost;

        public string ExchangeName { get; set; } = DefaultExchangeName;

        public ushort Port { get; set; } = 5672;

        public int QueueMessageExpires { get; set; } = 864000000;

        public Action<ConnectionFactory> ConnectionFactoryOptions { get; set; }
    }
}
