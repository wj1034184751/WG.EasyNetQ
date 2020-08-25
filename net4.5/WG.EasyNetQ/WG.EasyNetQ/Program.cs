using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WG.EasyNetQ.Core.Ioc;
using Microsoft.Extensions.DependencyInjection;
using WG.EasyNetQ.ETCore;
using Autofac;
using WG.EasyNetQ.Test;
using WG.EasyNetQ.DapperHelper;
using System.Reflection;
using WG.EasyNetQ.Extensions;
using WG.EasyNetQ.Uti;

namespace WG.EasyNetQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlConnectionTest.Test();
            //var builder = new ContainerBuilder();
            //builder.Register(d => new ETDbContext("Server=115.236.37.105,90;Database=Jiepei_Pcb;User Id=WKSite_Main;Password=WKSite_Main123456!@#;Connect Timeout=300")).InstancePerLifetimeScope();
            //builder.RegisterType<ETPublisher>().As<IETPublisher>();
            ////builder.RegisterGeneric(typeof(ETRepository<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterType<TestAutoFacClass>().As<ITestAutoFacClass>();
            //var classGenerics = typeof(Repository<,>).GetTypeInfo().Assembly.GetTypes()
            //                .Where(t => t.IsGenericType && !t.IsInterface);
            //foreach (var item in classGenerics)
            //{
            //    builder.RegisterGeneric(item).AsImplementedInterfaces().InstancePerLifetimeScope();
            //}
            //builder.RegisterAssemblyTypes(typeof(Repository<,>).GetTypeInfo().Assembly)
            //   .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)) || t.HasImplementdRawGeneric(typeof(IRepository<>)))
            //   .AsImplementedInterfaces()
            //   .InstancePerLifetimeScope();

            //IContainer container = builder.Build();
            //using (var scope = container.BeginLifetimeScope())
            //{
            //    var component = scope.Resolve<ITestAutoFacClass>();
            //    component.Print("wj", "v1");
            //    var com2 = scope.Resolve<IETPublisher>();
            //    com2.Send("", "");
            //}

            //IETRepository<CustomerQueue> repository = new ETRepository<CustomerQueue>(new ETDbContext("Server=115.236.37.105,90;Database=Jiepei_Pcb;User Id=WKSite_Main;Password=WKSite_Main123456!@#;Connect Timeout=300"));
            //repository.Insert(new CustomerQueue() { Version="dsf"});

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
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

            //EasyNetQs.Config();

            //EasyNetQs.SubscribeMessage<string>("customer_ponds_reserve_id2", "");
            //Console.WriteLine("First>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            IServiceCollection Services = new ServiceCollection();
            Services.AddCap(setup =>
            {
                //setup.UseSqlServer("Server=115.236.37.105,90;Database=Jiepei_Pcb;User Id=WKSite_Main;Password=WKSite_Main123456!@#;Connect Timeout=300");

                setup.UseSqlServer(d =>
                {
                    d.ConnectionString = "Server=115.236.37.105,90;Database=Jiepei_Pcb;User Id=WKSite_Main;Password=WKSite_Main123456!@#;Connect Timeout=300";
                });

                setup.UseRabbitMQ(option =>
                {
                    option.VirtualHost = "/";
                    option.HostName = "192.168.19.190";
                    option.Port = 5672;
                    option.UserName = "jp";
                    option.Password = "123456";
                });
            });

            Services.BeginRegister();

            IETPublisher _etBus = Services.ServiceProvider.GetRequiredService<IETPublisher>();

            //发送
            //for (var i = 0; i <= 10; i++)
            //{
            //    CustomerPonds customer = new CustomerPonds();
            //    customer.OperateName = i.ToString();
            //    customer.OperateId = i;
            //    customer.MarkTime = DateTime.Now;
            //    customer.MarkUserId = i;
            //    customer.CustomerId = i;
            //    var json = UnitHelper.Serialize(customer);

            //    _etBus.Send("test.v1", json);
            //}

            //接收
            _etBus.Receive("test.v1", d =>
             {
                 Console.WriteLine("接收"+d);
                 //throw new Exception("出错!");
             });


            //IETPublisher _etBus=Services
            //Console.WriteLine("发送Send");
            //for (int i = 0; i < 2; i++)
            //{
            //    //EasyNetQs.client.Publish("heelo rabbit!" + i, "customer_ponds_reserve_id2");
            //    CustomerPonds model = new CustomerPonds();
            //    model.CustomerId = i;
            //    model.MarkUserId = i;
            //    var json = Uti.UnitHelper.Serialize(model);
            //    EasyNetQs.SendMessage("test.wj.test", json);
            //}

            //EasyNetQs.ReceiveMessage("customer.ponds.reserve", d =>
            //{
            //    Console.WriteLine(d);
            //    //throw new Exception("出错!");
            //});

            //EasyNetQs.SubscribeMessage("test.wj.sub", d =>
            //{
            //    Console.WriteLine(d);
            //});

            ////EasyNetQs.SubscribeMessage("test.wj.sub1", d =>
            ////{
            ////    //throw new Exception();
            ////    Console.WriteLine(d);
            ////    //throw new Exception("出错!");
            ////});
            //Console.WriteLine("发送Publicsh");
            //for (int i = 0; i < 2; i++)
            //{
            //    CustomerPonds model = new CustomerPonds();
            //    model.CustomerId = i;
            //    model.MarkUserId = i;
            //    model.MarkTime = DateTime.Now;
            //    var json = Uti.UnitHelper.Serialize(model);
            //    EasyNetQs.PublishMessage("test.wj.sub", json);
            //}

            //Thread.Sleep(5000);
            //for (int i = 0; i < 2; i++)
            //{
            //    CustomerPonds model = new CustomerPonds();
            //    model.CustomerId = i;
            //    model.MarkUserId = i;
            //    var json = Uti.UnitHelper.Serialize(model);
            //    EasyNetQs.PublishMessage("test.wj.sub", json);
            //}

            //Thread.Sleep(10000);
            //  for (int i = 0; i < 2; i++)
            //{
            //    CustomerPonds model = new CustomerPonds();
            //    model.CustomerId = i;
            //    model.MarkUserId = i;
            //    var json = Uti.UnitHelper.Serialize(model);
            //    EasyNetQs.PublishMessage("test.wj.sub", json);
            //}

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
