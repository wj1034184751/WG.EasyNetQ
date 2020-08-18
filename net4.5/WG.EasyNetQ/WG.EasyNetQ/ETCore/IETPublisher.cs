using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.ETCore
{
    public interface IETPublisher
    {
        void Send(string name, string message);

        void Send<T>(string name, T message);

        void Publish(string name, string message);

        void Publish<T>(string name, T message);
    }
}
