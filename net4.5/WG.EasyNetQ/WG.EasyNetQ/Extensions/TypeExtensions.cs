using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasImplementdRawGeneric(this Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            return type.GetInterfaces().Any(d => generic == (d.IsGenericType ? d.GetGenericTypeDefinition() : d));
        }
    }
}
