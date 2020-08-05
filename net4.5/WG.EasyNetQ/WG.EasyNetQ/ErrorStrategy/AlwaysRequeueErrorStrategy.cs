using EasyNetQ.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.DapperHelper;
using WG.EasyNetQ.MqEnum;
using WG.EasyNetQ.Uti;

namespace WG.EasyNetQ.ErrorStrategy
{
    /// <summary>
    /// 发生异常时,将消息返回到原有队列，进行再次的数据处理
    /// </summary>
    public class AlwaysRequeueErrorStrategy : IConsumerErrorStrategy
    {
        private object _lock = new object();

        public void Dispose()
        {
        }

        public AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception)
        {
            lock (_lock)
            {
                if (!(context.Body != null && context.Body.Length > 0))
                    return AckStrategies.NackWithoutRequeue;
                var messageBody = Encoding.UTF8.GetString(context.Body);
                var info = context.Info;
                var model = UnitHelper.DeserializeObject<CustomerQueue>(messageBody);
                if (model == null || string.IsNullOrWhiteSpace(model.QueueValue))
                    return AckStrategies.NackWithoutRequeue;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                //todo:记录数据库
                model.QueueName = info.RoutingKey;
                model.IsConsume = (int)MqStatus.Failed;
                model.UpdateTime = DateTime.Now;
                model.Version = UnitHelper.GetVersion(info.RoutingKey, content.Id);
                content.Content = content.Content;
                content.ExceptionMessage = new ExceptionMessage();
                content.ExceptionMessage.Message = exception.Message;
                content.ExceptionMessage.Source = info.Queue;
                model.QueueValue = UnitHelper.Serialize(content);
                var result = DapperSqlHelper.GetByVersion(new CustomerQueue() { Version = model.Version });
                if (result != null)
                {
                    //重发次数
                    if (result.CetryCount >= 5)
                    {
                        return AckStrategies.NackWithoutRequeue;
                    }

                    model.CetryCount = result.CetryCount.HasValue ? result.CetryCount + 1 : 1;
                    DapperSqlHelper.Update(new CustomerQueue() { Version = model.Version, QueueName = model.QueueName, QueueValue = model.QueueValue, IsConsume = model.IsConsume, UpdateTime = model.UpdateTime, CetryCount = model.CetryCount });
                }
                else
                {
                    DapperSqlHelper.Insert(model);
                }

                return AckStrategies.NackWithRequeue;
            }
        }
        public AckStrategy HandleConsumerCancelled(ConsumerExecutionContext context)
        {
            //return AckStrategies.NackWithoutRequeue;
            return AckStrategies.NackWithRequeue;
        }
    }
}
