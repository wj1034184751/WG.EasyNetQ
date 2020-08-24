using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.ET.RabbitMQ
{
    public interface IRabbitMQClient
    {
        IBus Client { get; }
    }
}
