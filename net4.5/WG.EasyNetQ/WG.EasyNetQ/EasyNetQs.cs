using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.SystemMessages;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WG.EasyNetQ.DapperHelper;
using WG.EasyNetQ.ErrorStrategy;
using WG.EasyNetQ.MqEnum;
using WG.EasyNetQ.Uti;

namespace WG.EasyNetQ
{
    public class EasyNetQs
    {
        private static object _obj = new object();

        private static IBus _client { get; set; }

        /// <summary>
        /// 错误队例
        /// </summary>
        private const string ErrorQueue = "EasyNetQ_Default_Error_Queue";

        public static IBus client
        {
            get
            {
                if (_client == null)
                 {
                    lock (_obj)
                    {
                        if (_client == null)
                        {
                            string connStr = ConfigUtils.GetValue<string>("RabbitMq");
                            _client = RabbitHutch.CreateBus(connStr);
                            //_client = RabbitHutch.CreateBus(connStr, x => x.Register<IConsumerErrorStrategy>(d => new AlwaysRequeueErrorStrategy()));
                        }
                    }
                }

                HandleErrors();
                return _client;
            }
        }

        //static EasyNetQs()
        //{
        //    if (_client == null)
        //    {
        //        lock (_obj)
        //        {
        //            if (_client == null)
        //            {
        //                string connStr = ConfigUtils.GetValue<string>("RabbitMq");
        //                _client = RabbitHutch.CreateBus(connStr);
        //                //_client = RabbitHutch.CreateBus(connStr, x => x.Register<IConsumerErrorStrategy>(d => new AlwaysRequeueErrorStrategy()));
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 点对点发送
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <param name="message">json对象</param>
        public static void SendMessage(string queue, string message)
        {
            //持久化插入数据库
            CustomerQueue model = new CustomerQueue();
            QueueValue content = new QueueValue();
            model.QueueName = queue;
            model.IsConsume = (int)MqStatus.Wait;
            model.UpdateTime = DateTime.Now;
            content.Id = Uti.SnowflakeId.Default().NextId();
            content.Content = message;
            model.QueueValue = UnitHelper.Serialize(content);
            model.Version = UnitHelper.GetVersion(queue, content.Id);
            lock (_obj)
            {
                var result= DapperSqlHelper.GetCountByVersion("select Count(Id) from  [CustomerQueue] where Version=@Version", new CustomerQueue { Version = model.Version });
                if (result == 0)
                {
                    DapperSqlHelper.Insert(model);
                    client.Send<CustomerQueue>(queue, model);
                }
            }
        }

        /// <summary>
        /// 点对点发送
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <param name="message">对象</param>
        public static void SendMessage<T>(string queue, T message) where T : class, new()
        {
            SendMessage(queue, UnitHelper.Serialize(message));

            #region
            //CustomerQueue model = new CustomerQueue();
            //model.QueueName = queue;
            //model.QueueValue = Serialize(message);
            //model.IsConsume = 0;
            //model.UpdateTime = DateTime.Now;
            //model.Version = GetVersion(queue, model.QueueValue);
            //Repository<CustomerQueue> repository = new Repository<CustomerQueue>();
            //lock (_obj)
            //{
            //    var result = repository.GetByVersion("select Count(Id) from  [CustomerQueue] where Version=@Version", new CustomerQueue { Version = model.Version });
            //    if (result == 0)
            //    {
            //        repository.Insert(model);
            //        client.Send<T>(queue, message);
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="act"></param>
        public static void ReceiveMessage(string queue, Action<string> act)
        {
            client.Receive<CustomerQueue>(queue, onMessage =>
            {
                var model = onMessage;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                act.Invoke(content.Content);

                //持久化插入数据库
                #region
                var version = UnitHelper.GetVersion(queue, content.Id);
                DapperSqlHelper.Execute("Update [CustomerQueue] set [IsConsume]=@IsConsume where Version=@Version", new CustomerQueue { IsConsume = (int)MqStatus.Succeeded, Version = version });
                #endregion
            });
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="act"></param>
        public static void ReceiveMessage<T>(string queue, Action<T> act) where T : class
        {
            client.Receive<CustomerQueue>(queue, onMessage =>
            {
                var model = onMessage;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueName);
                act.Invoke(UnitHelper.DeserializeObject<T>(content.Content));

                //持久化插入数据库
                #region
                var version = UnitHelper.GetVersion(queue, content.Id);
                DapperSqlHelper.Execute("Update [CustomerQueue] set [IsConsume]=@IsConsume where Version=@Version", new CustomerQueue { IsConsume = (int)MqStatus.Succeeded, Version = version });
                #endregion
            });
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        public static void PublishMessage(string queue, string message)
        {
            //持久化插入数据库
            #region
            #endregion

            client.Publish(message, queue);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        public static void PublishMessage<T>(string queue, T message) where T : class
        {
            client.Publish<T>(message, queue);
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        public static void SubscribeMessage(string queue)
        {
            client.Subscribe<string>(queue, onMessage =>
            {
                Console.WriteLine(onMessage);
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        public static void SubscribeMessage<T>(string queue,Action<T> act) where T : class
        {
            #region
            //CustomerQueue model = new CustomerQueue();
            //model.QueueName = queue;
            //model.QueueValue = Serialize(message);
            //model.IsConsume = 0;
            //model.UpdateTime = DateTime.Now;
            //model.Version = GetVersion(queue, model.QueueValue);
            //Repository<CustomerQueue> repository = new Repository<CustomerQueue>();
            //lock (_obj)
            //{
            //    var result = repository.GetByVersion("select Count(Id) from  [CustomerQueue] where Version=@Version", new CustomerQueue { Version = model.Version });
            //    if (result == 0)
            //    {
            //        repository.Insert(model);
            //        client.Send<T>(queue, message);
            //    }
            //}
            #endregion
            //_client.Subscribe<T>(queue, d =>
            //{
            //    Console.WriteLine(d);
            //});

            client.Subscribe<T>(queue, act);
        }

        /// <summary>
        /// 异常
        /// </summary>
        public static void HandleErrors()
        {
            Action<IMessage<Error>, MessageReceivedInfo> handleErrorMessage = (msg, info) =>
            {
                Console.WriteLine(msg.Body.Message);
                var model = UnitHelper.DeserializeObject<CustomerQueue>(msg.Body.Message);
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);

                //todo:记录数据库
                model.QueueName = info.RoutingKey;
                model.IsConsume = (int)MqStatus.Failed;
                model.UpdateTime = DateTime.Now;
                model.Version = UnitHelper.GetVersion(info.RoutingKey, content.Id);
                content.Content = content.Content;
                content.ExceptionMessage = new ExceptionMessage();
                content.ExceptionMessage.Message = msg.Body.Exception;
                content.ExceptionMessage.Source = info.Queue;
                model.QueueValue = UnitHelper.Serialize(content);
                var result = DapperSqlHelper.GetByVersion("select * from [CustomerQueue] WHERE [Version]=@Version", new CustomerQueue() { Version = model.Version });
                if (result != null)
                {
                    model.CetryCount = model.CetryCount.HasValue ? model.CetryCount + 1 : 1;
                    DapperSqlHelper.Execute(@"UPDATE [CustomerQueue]
                                       SET[QueueName] =@QueueName
                                          ,[Version] =@Version
                                          ,[QueueValue] =@QueueValue
                                          ,[IsConsume] =@IsConsume
                                          ,[UpdateTime] =@UpdateTime 
                                          ,[CetryCount] =@CetryCount WHERE Version=@Version", model);
                }
                else
                {
                    DapperSqlHelper.Insert(model);
                }
            };

            IQueue queue = new Queue(ErrorQueue, false);
            client.Advanced.Consume(queue, handleErrorMessage);
        }

        /// <summary>
        /// 重试
        /// </summary>
        public static void RetrySend()
        {
            var list = DapperSqlHelper.GetList("SELECT  top 10 * FROM  [CustomerQueue] WHERE IsConsume=2 ORDER BY UpdateTime");
            if (list.Any())
            {
                foreach (var item in list)
                {
                    RetrySendMessage(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendModel"></param>
        public static void RetrySendMessage(CustomerQueue sendModel)
        {
            //持久化插入数据库
            lock (_obj)
            {
                var result = DapperSqlHelper.Execute("update  [CustomerQueue] set [CetryCount]=ISNULL([CetryCount],0)+1 where Version=@Version", new CustomerQueue { Version = sendModel.Version });
                if (result >= 1)
                {
                    client.Send(sendModel.QueueName, sendModel.QueueValue);
                }
            }
        }

        /// <summary>
        /// 删除七日前的数据
        /// </summary>
        public static void DelQueue()
        {
            var result = DapperSqlHelper.Execute("delete  from  [CustomerQueue] where UpdateTime<=@UpdateTime", new CustomerQueue { UpdateTime = DateTime.Now.AddDays(-7) });
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public static void Config()
        {
            //删除数据
            DelQueue();

            //重发失败消息
            Thread th = new Thread(() =>
            {
                while (true)
                {
                    RetrySend();
                    Thread.Sleep(1000);
                }
            });
            th.Start();
        }

        public static void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}
