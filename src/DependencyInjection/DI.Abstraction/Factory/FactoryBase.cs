using System;
using System.Collections.Generic;
using System.Linq;

namespace VectronsLibrary.DI.Factory
{
    /// <summary>
    /// Base class for making custom factories.
    /// </summary>
    /// <typeparam name="T">The type this factory will create.</typeparam>
    public abstract class FactoryBase<T> : IFactory<T>
        where T : class
    {
        private readonly IRegisteredTypes<T> registeredTypes;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryBase{T}"/> class.
        /// </summary>
        /// <param name="registeredTypes">The <see cref="IRegisteredTypes{T}"/> used to check what types inherit from <typeparamref name="T"/>.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to instantiate services.</param>
        public FactoryBase(IRegisteredTypes<T> registeredTypes, IServiceProvider serviceProvider)
        {
            this.registeredTypes = registeredTypes;
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public virtual T Value => GetValue(Name);

        /// <summary>
        /// Gets the name of the type we are looking for.
        /// </summary>
        protected abstract string Name
        {
            get;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetItemNames()
            => registeredTypes.Items.Select(x => x.Name);

        /// <inheritdoc />
        public T GetValue(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Can't resolve item without name", nameof(name));
            }

            var foundType = registeredTypes.Items.Where(x => x.Name == name).FirstOrDefault();
            if (foundType != null && serviceProvider.GetService(foundType) is T implementation)
            {
                return implementation;
            }

            throw new ArgumentException($"{name} is not found", nameof(name));
        }
    }
}