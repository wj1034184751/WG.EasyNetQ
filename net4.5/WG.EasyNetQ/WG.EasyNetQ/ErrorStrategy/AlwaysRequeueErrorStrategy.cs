using EasyNetQ.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.ErrorStrategy
{
    /// <summary>
    /// 发生异常时,将消息返回到原有队列，进行再次的数据处理
    /// </summary>
    public class AlwaysRequeueErrorStrategy : IConsumerErrorStrategy
    {
        public void Dispose()
        {
        }

        public AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception)
        {
            return AckStrategies.NackWithRequeue;
        }
        public AckStrategy HandleConsumerCancelled(ConsumerExecutionContext context)
        {
            return AckStrategies.NackWithRequeue;
        }
    }
}
