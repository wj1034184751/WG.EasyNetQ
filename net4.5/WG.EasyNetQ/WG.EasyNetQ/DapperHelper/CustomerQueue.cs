﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.MqEnum;

namespace WG.EasyNetQ.DapperHelper
{
    [Serializable]
    public partial class CustomerQueue
    {
        public int Id { get; set; }

        public string Version { get; set; }
        public string QueueName { get; set; }
        public string QueueValue { get; set; }
        public Nullable<int> IsConsume { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<int> CetryCount { get; set; }
    }

    [Serializable]
    public partial class QueueValue
    {
        public long Id { get; set; }

        public QueueType QueueType { get; set; }

        public string Content { get; set; }

        public ExceptionMessage ExceptionMessage { get; set; }
    }

    [Serializable]
    public partial class ExceptionMessage
    {
        public string Source { get; set; }

        public string Message { get; set; }

        public string InnerMessage { get; set; }
    }
}
