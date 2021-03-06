﻿using EasyNetQ;
using EasyNetQ.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.DapperHelper;
using WG.EasyNetQ.ErrorStrategy;
using WG.EasyNetQ.ET.RabbitMQ.Core;

namespace WG.EasyNetQ.ET.RabbitMQ
{
    public class RabbitMQClient : IRabbitMQClient
    {
        private const string ErrorQueue = "EasyNetQ_Default_Error_Queue";
        private readonly RabbitMQOptions _rabbitMQOptions;
        private readonly IETRepository<CustomerQueue> _eTRepository;
        public IBus _client;

        public IBus Client
        {
            get
            {
                if (_client == null)
                {
                    _client = RabbitHutch.CreateBus(new ConnectionConfiguration
                    {
                        Port = _rabbitMQOptions.Port,
                        Hosts = new List<HostConfiguration>() { new HostConfiguration() { Host = _rabbitMQOptions.HostName } },
                        VirtualHost = _rabbitMQOptions.VirtualHost,
                        UserName = _rabbitMQOptions.UserName,
                        Password = _rabbitMQOptions.Password
                    }, x => x.Register<IConsumerErrorStrategy>(d => new AlwaysRequeueErrorStrategy(_eTRepository))); //注册异常重发
                }

                return _client;
            }
        }

        public RabbitMQClient(RabbitMQOptions rabbitMQOptions,
                              IETRepository<CustomerQueue> eTRepository)
        {
            this._rabbitMQOptions = rabbitMQOptions;
            this._eTRepository = eTRepository;
        }
    }
}
