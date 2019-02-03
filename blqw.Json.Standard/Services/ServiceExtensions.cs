using blqw.ConvertServices;
using blqw.DI;
using blqw.JsonServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace blqw.JsonServices
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddJsonService(this IServiceCollection services)
        {
            var types = typeof(Json).Assembly.SafeGetTypes();
            types = types.Where(x => typeof(IJsonWriter).IsAssignableFrom(x) && x.IsClass && x.Instantiable());
            types.ForEach(x => services.AddSingleton(typeof(IJsonWriter), x));
            services.AddSingleton<IConvertorSelector, ConvertorSelector>();
            services.AddSingleton<ConvertSettings>();
            services.AddSingleton<IComparer<Type>>(TypeComparer.Instance);
            services.AddSingleton(p => new JsonWriterSelector(p));
            return services;
        }

    }
}
