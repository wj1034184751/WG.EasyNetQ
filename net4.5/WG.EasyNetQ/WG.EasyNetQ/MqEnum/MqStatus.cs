using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.MqEnum
{
    public enum MqStatus
    {
        Wait = 0,
        Succeeded = 1,
        Failed = -1
    }
}
