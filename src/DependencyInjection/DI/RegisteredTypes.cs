using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// The default implementation of <see cref="IRegisteredTypes{T}"/>.
    /// </summary>
    /// <typeparam name="T">The base type to look for.</typeparam>
    [Singleton]
    public class RegisteredTypes<T> : IRegisteredTypes<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredTypes{T}"/> class.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> used to built the <see cref="IServiceProvider"/>.</param>
        public RegisteredTypes(IServiceCollection serviceCollection)
        {
            Items = serviceCollection
                .Where(x => x.ServiceType == typeof(T) || x.ServiceType.GetInterfaces().Contains(typeof(T)))
                .Select(x => x.ImplementationType)
                .Where(x => x != null)
                .Distinct();
        }

        /// <inheritdoc />
        public IEnumerable<Type> Items
        {
            get;
            set;
        }
    }
}