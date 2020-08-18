using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WG.EasyNetQ.Core;
using WG.EasyNetQ.Core.Ioc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ServiceCollection;

        public static ETBuilder AddCap(this IServiceCollection services,Action<ETOptions>setupAction)
        {
            if(setupAction==null)
            {
                throw new ArgumentException(nameof(setupAction));
            }

            ServiceCollection = services;

            var options = new ETOptions();
            setupAction(options);
            foreach(var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.AddSingleton(options);

            return new ETBuilder(services);
        }
    }
}
