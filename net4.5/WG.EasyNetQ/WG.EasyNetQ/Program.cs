using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //var str = ConfigurationManager.ConnectionStrings["ZmDatabaseEntities1"].ToString();
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("data source", "");
            //dic.Add("initial catalog", "");
            //dic.Add("Password", "");
            //dic.Add("User ID", "");

            //EasyNetQs.HandleErrors();
            //EasyNetQs.HandleErrors();
            //EasyNetQs.SubscribeMessage<string>("customer_ponds_reserve_id2", d =>
            // {
            //     Console.WriteLine("接收!");
            //     Console.WriteLine(d);
            //     throw new Exception("ddd");
            // });



            //EasyNetQs.SubscribeMessage<string>("customer_ponds_reserve_id2", "");
            //Console.WriteLine("First>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            for (int i = 0; i < 2; i++)
            {
                //EasyNetQs.client.Publish("heelo rabbit!" + i, "customer_ponds_reserve_id2");
                CustomerPonds model = new CustomerPonds();
                model.CustomerId = i;
                model.MarkUserId = i;
                var json = Uti.UnitHelper.Serialize(model);
                EasyNetQs.SendMessage("test.wj.test", json);
            }

            EasyNetQs.ReceiveMessage("test.wj.test", d =>
            {
                Console.WriteLine(d);
                throw new Exception("出错!");
            });

            for (int i = 0; i < 2; i++)
            {
                //EasyNetQs.client.Publish("heelo rabbit!" + i, "customer_ponds_reserve_id2");
                CustomerPonds model = new CustomerPonds();
                model.CustomerId = i+1;
                model.MarkUserId = i+1;
                var json = Uti.UnitHelper.Serialize(model);
                EasyNetQs.SendMessage("test.wj.test", json);
            }


            //Console.WriteLine("Rabbit >>>>>>>>>>>>>>>>>>>>>");
            //EasyNetQs.SubscribeMessage<string>("customer_ponds_reserve_id");
            //Repository<CustomerQueue> repository = new Repository<CustomerQueue>();
            //var tb1= repository.GetList();
            //var tb = sqlHelper.ExecuteDataTable("SELECT * FROM dbo.CustomerQueue");
            //Repository<CustomerQueue> repository = new Repository<CustomerQueue>();
            //var res = EasyNetQs.GetVersion("customer.ponds.reserve", "12312312312");
            //var result = repository.GetByVersion("select Count(Id) from  [CustomerQueue] where Version=@Version", new CustomerQueue { Version = "EB2E796BD9D058F64E7D51091C65E72F" });
            //return;

            //EasyNetQs.ReceiveMessage("EasyNetQ_Default_Error_Queue", Printf);\
            //EasyNetQs.PublishMessage("wj.test.queue", "w");
            //Thread th = new Thread(() =>
            //  {
            //      while (true)
            //      {
            //          Console.WriteLine("开始查询");
            //          EasyNetQs.RetrySend();
            //          Console.WriteLine("休息1秒");
            //          Thread.Sleep(1000);
            //      }
            //  });

            //th.Start();

            //EasyNetQs.SubscribeMessage<string>("wj.test.queue");
            //EasyNetQs.PublishMessage("wj.test.queue", "w");
            return;
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    CustomerPonds t = new CustomerPonds();
                    t.OperateId = i;
                    t.OperateName = string.Format("王{0}", i);
                    t.MarkUserId = i;
                        //t.MarkTime = DateTime.Now;
                        t.CustomerId = i;
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                        //EasyNetQs.SendMessages("customer.ponds.reserve", json);
                        EasyNetQs.SendMessage<CustomerPonds>("customer.ponds.reserve", t);
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    CustomerPonds t = new CustomerPonds();
                    t.OperateId = i;
                    t.OperateName = string.Format("王{0}", i);
                    t.MarkUserId = i;
                        //t.MarkTime = DateTime.Now;
                        t.CustomerId = i;
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                        //EasyNetQs.SendMessages("customer.ponds.reserve", json);
                        EasyNetQs.SendMessage<CustomerPonds>("customer.ponds.reserve", t);
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    CustomerPonds t = new CustomerPonds();
                    t.OperateId = i;
                    t.OperateName = string.Format("王{0}", i);
                    t.MarkUserId = i;
                        //t.MarkTime = DateTime.Now;
                        t.CustomerId = i;
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                        //EasyNetQs.SendMessages("customer.ponds.reserve", json);
                        EasyNetQs.SendMessage<CustomerPonds>("customer.ponds.reserve", t);
                }
            }));

            Task.WaitAll(tasks.ToArray());

            //EasyNetQs.ReceiveMessage("customer.ponds.reserve", Printf);
            EasyNetQs.ReceiveMessage<CustomerPonds>("customer.ponds.reserve", Printf);
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            EasyNetQs.ReceiveMessage<CustomerPonds>("customer.ponds.reserve", Printf);
        }

        static void Printf(string onMessage)
        {
            Console.WriteLine(onMessage);
        }

        static void Printf(CustomerPonds onMessage)
        {
            Console.WriteLine(onMessage.OperateName);
        }
    }

    public class CustomerPonds
    {
        public int? OperateId { get; set; }

        public string OperateName { get; set; }

        public int? MarkUserId { get; set; }

        public DateTime MarkTime { get; set; }

        public List<int> Customers { get; set; }

        public int CustomerId { get; set; }
    }
}
