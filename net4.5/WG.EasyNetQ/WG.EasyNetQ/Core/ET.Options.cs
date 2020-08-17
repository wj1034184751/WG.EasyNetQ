using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Extensions;

namespace WG.EasyNetQ.Core
{
    public class ETOptions
    {
        public const int DefaultSucceedMessageExpirationAfter = 24 * 3600;

        public const string DefaultVersion = "v1";

        internal IList<IETOptionsExtension> Extensions { get; }

        public ETOptions()
        {
            Extensions = new List<IETOptionsExtension>();
        }
    }
}
