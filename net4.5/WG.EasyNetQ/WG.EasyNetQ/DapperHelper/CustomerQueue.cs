using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.DapperHelper
{
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
}
