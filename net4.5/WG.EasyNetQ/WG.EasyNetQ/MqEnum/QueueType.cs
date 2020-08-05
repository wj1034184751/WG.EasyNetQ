using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.MqEnum
{
    /// <summary>
    /// 模式
    /// </summary>
    public enum QueueType
    {
        /// <summary>
        /// 点对点
        /// </summary>
        Queue=1,
        /// <summary>
        /// 订阅发布
        /// </summary>
        Topic=2
    }
}
