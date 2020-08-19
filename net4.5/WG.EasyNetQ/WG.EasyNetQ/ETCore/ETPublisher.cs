using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.DapperHelper;
using WG.EasyNetQ.MqEnum;
using WG.EasyNetQ.Uti;

namespace WG.EasyNetQ.ETCore
{
    public class ETPublisher : IETPublisher
    {
        private readonly IETRepository<CustomerQueue> _eTRepository;

        public ETPublisher(IETRepository<CustomerQueue> eTRepository)
        {
            this._eTRepository = eTRepository;
        }

        private static object _obj = new object();

        public void Publish(string name, string message)
        {
            this._eTRepository.Insert(new CustomerQueue() { QueueName = name, QueueValue = message });
        }

        public void Publish<T>(string name, T message)
        {
            throw new NotImplementedException();
        }

        public void Send<T>(string name, T message)
        {
            throw new NotImplementedException();
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
                var result = DapperSqlHelper.GetCountByVersion(new CustomerQueue { Version = model.Version });
                if (result == 0)
                {
                    this._eTRepository.Insert(model);
                    //DapperSqlHelper.Insert(model);
                    //client.Send<CustomerQueue>(queue, model);
                }
            }
        }
    }
}
