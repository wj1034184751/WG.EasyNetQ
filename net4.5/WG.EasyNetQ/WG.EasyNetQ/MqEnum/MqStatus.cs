using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.MqEnum
{
    public enum MqStatus
    {
        //等待
        Wait = 0,
        //成功
        Succeeded = 1,
        //失败
        Failed = -1
    }
}
