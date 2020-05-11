using System;
using System.Collections.Generic;
using System.Linq;

namespace VectronsLibrary.DI.Factory
{
    public abstract class FactoryBase<T> : IFactory<T>
        where T : class
    {
        private readonly IRegisteredTypes<T> registeredTypes;
        private readonly IServiceProvider serviceProvider;

        public FactoryBase(IRegisteredTypes<T> registeredTypes, IServiceProvider serviceProvider)
        {
            this.registeredTypes = registeredTypes;
            this.serviceProvider = serviceProvider;
        }

        public virtual T Value => GetValue(Name);

        protected abstract string Name
        {
            get;
        }

        public IEnumerable<string> GetItemNames() => registeredTypes.Items.Select(x => x.Name);

        public T GetValue(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Can't resolve item without name", nameof(name));
            }

            var foundType = registeredTypes.Items.Where(x => x.Name == name).First() ??
                throw new ArgumentException($"{name} is not found", nameof(name));
            return (T)serviceProvider.GetService(foundType);
        }
    }
}