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
    public abstract class ETPublisher : IETPublisher
    {
        private readonly IRepository<CustomerQueue> _IRepository;

        public ETPublisher(IRepository<CustomerQueue> IRepository)
        {
            this._IRepository = IRepository;
        }
        private static object _obj = new object();

        public void Publish(string name, string message)
        {
            throw new NotImplementedException();
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
                Id= Uti.SnowflakeId.Default().NextId(),
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
                    this._IRepository.Insert(model);
                    //DapperSqlHelper.Insert(model);
                    //client.Send<CustomerQueue>(queue, model);
                }
            }
        }
    }
}
