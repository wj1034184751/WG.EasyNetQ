using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.DapperHelper;
using WG.EasyNetQ.ET.RabbitMQ;
using WG.EasyNetQ.MqEnum;
using WG.EasyNetQ.Uti;

namespace WG.EasyNetQ.ETCore
{
    public class ETPublisher : IETPublisher
    {
        private readonly IETRepository<CustomerQueue> _eTRepository;
        private readonly IRabbitMQClient  _rabbitMQClient;

        public ETPublisher(IETRepository<CustomerQueue> eTRepository,
                           IRabbitMQClient rabbitMQClient)
        {
            this._eTRepository = eTRepository;
            this._rabbitMQClient = rabbitMQClient;
        }

        private static object _obj = new object();

        public void Publish(string name, string message)
        {
            //持久化插入数据库
            var content = new QueueValue()
            {
                Id = Uti.SnowflakeId.Default().NextId(),
                Content = message,
                QueueType = QueueType.Topic,
            };

            var model = new CustomerQueue
            {
                QueueName = name,
                IsConsume = (int)MqStatus.Wait,
                UpdateTime = DateTime.Now,
                QueueValue = UnitHelper.Serialize(content),
                Version = UnitHelper.GetVersion(name, content.Id)
            };

            lock (_obj)
            {
                var result = this._eTRepository.Insert(new CustomerQueue() { QueueName = name, QueueValue = message });
                if (result == 0)
                {
                    DapperSqlHelper.Insert(model);
                    this._rabbitMQClient.Client.Publish<CustomerQueue>(model, name);
                }
            }
        }

        public void Publish<T>(string name, T message)
        {
            Publish(name, UnitHelper.Serialize(message));
        }

        public void Send<T>(string name, T message)
        {
            Send(name, UnitHelper.Serialize(message));
        }

        public void Send(string name, string message)
        {
            //持久化插入数据库
            var content = new QueueValue()
            {
                Id = Uti.SnowflakeId.Default().NextId(),
                Content = message,
                QueueType = QueueType.Queue,
            };

            var model = new CustomerQueue()
            {
                QueueName = name,
                IsConsume = (int)MqStatus.Wait,
                UpdateTime = DateTime.Now,
                QueueValue = UnitHelper.Serialize(content),
                Version = UnitHelper.GetVersion(name, content.Id)
            };

            lock (_obj)
            {
                var result = _eTRepository.GetCountByVersion(new CustomerQueue { Version = model.Version });
                if (result == 0)
                {
                    this._eTRepository.Insert(model);
                    this._rabbitMQClient.Client.Send<CustomerQueue>(name, model);
                }
            }
        }

        public void Receive<T>(string name, Action<T> act)
        {
            this._rabbitMQClient.Client.Receive<CustomerQueue>(name, onMessage =>
            {
                var model = onMessage;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueName);
                act.Invoke(UnitHelper.DeserializeObject<T>(content.Content));

                //持久化插入数据库
                #region
                var version = UnitHelper.GetVersion(name, content.Id);
                this._eTRepository.UpdateState(new CustomerQueue { IsConsume = (int)MqStatus.Succeeded, Version = version });
                #endregion
            });
        }

        public void Receive(string name, Action<string> act)
        {
            this._rabbitMQClient.Client.Receive<CustomerQueue>(name, onMessage =>
            {
                var model = onMessage;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                act.Invoke(content.Content);

                //持久化插入数据库
                #region
                var version = UnitHelper.GetVersion(name, content.Id);
                this._eTRepository.UpdateState(new CustomerQueue { IsConsume = (int)MqStatus.Succeeded, Version = version });
                #endregion
            });
        }

        public void Subscribe<T>(string name, Action<T> act)
        {
            this._rabbitMQClient.Client.Subscribe<CustomerQueue>(name, noMessage => Task.Factory.StartNew(() =>
            {
                var model = noMessage;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                act.Invoke(UnitHelper.DeserializeObject<T>(content.Content));
            }).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    //持久化插入数据库
                    #region
                    var model = noMessage;
                    var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                    var version = UnitHelper.GetVersion(name, content.Id);
                    this._eTRepository.UpdateState(new CustomerQueue { IsConsume = (int)MqStatus.Succeeded, Version = version });
                    #endregion
                }
            }));
        }

        public void Subscribe(string name, Action<string> act)
        {
            this._rabbitMQClient.Client.Subscribe<CustomerQueue>(name, noMessage => Task.Factory.StartNew(() =>
            {
                var model = noMessage;
                var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                act.Invoke(content.Content);
            }).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    //持久化插入数据库
                    #region
                    var model = noMessage;
                    var content = UnitHelper.DeserializeObject<QueueValue>(model.QueueValue);
                    var version = UnitHelper.GetVersion(name, content.Id);
                    this._eTRepository.UpdateState(new CustomerQueue { IsConsume = (int)MqStatus.Succeeded, Version = version });
                    #endregion
                }
            }));
        }
    }
}
