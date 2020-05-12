using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VectronsLibrary.DI
{
    [Singleton]
    public class RegisteredTypes<T> : IRegisteredTypes<T>
    {
        public RegisteredTypes(IServiceCollection serviceCollection)
        {
            Items = serviceCollection
             .Where(x => x.ServiceType == typeof(T) || x.ServiceType.GetInterfaces().Contains(typeof(T)))
             .Select(x => x.ImplementationType)
             .Where(x => x != null)
             .Distinct();
        }

        public IEnumerable<Type> Items
        {
            get;
            set;
        }
    }
}