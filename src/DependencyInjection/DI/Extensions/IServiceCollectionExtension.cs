using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VectronsLibrary.DI
{
    public static class IServiceCollectionExtension
    {
        private const string AddMessage = "Adding contract type: {0}, with implementation type: {1} as {2}";
        private const string GenericAddMessage = "Adding contract type: {0}, with generic implementation type: {1} as {2}";
        private const string IgnoreMessage = "Ignoring contract type: {0}, with implementation type: {1}";
        private static ILogger logger = NullLogger.Instance;

        public static IServiceCollection Add(this IServiceCollection serviceDescriptors, Type contractType, Type implementation, ServiceLifetime serviceLifetime)
        {
            if (implementation.IsGenericTypeDefinition)
            {
                logger.LogDebug(GenericAddMessage, contractType.FullName, implementation.FullName, serviceLifetime);
                serviceDescriptors.Add(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
                return serviceDescriptors;
            }

            logger.LogDebug(AddMessage, contractType.FullName, implementation.FullName, serviceLifetime);

            if (contractType == implementation)
            {
                serviceDescriptors.Add(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
                return serviceDescriptors;
            }

            serviceDescriptors.Add(ServiceDescriptor.Describe(contractType, x => x.GetService(implementation), serviceLifetime));
            serviceDescriptors.Add(ServiceDescriptor.Describe(implementation, implementation, serviceLifetime));
            return serviceDescriptors;
        }

        public static IServiceCollection AddAssemblyResolver(this IServiceCollection serviceDescriptors)
            => serviceDescriptors.TryAddByAttribute(typeof(IAssemblyResolver), typeof(AssemblyResolver));

        public static IServiceCollection AddByAttribute(this IServiceCollection serviceDescriptors, Type contractType, Type implementation)
        {
            if (Attribute.IsDefined(implementation, typeof(SingletonAttribute)) ||
                Attribute.IsDefined(contractType, typeof(SingletonAttribute)))
            {
                return serviceDescriptors.Add(contractType, implementation, ServiceLifetime.Singleton);
            }
            else if (Attribute.IsDefined(implementation, typeof(TransientAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(TransientAttribute)))
            {
                return serviceDescriptors.Add(contractType, implementation, ServiceLifetime.Transient);
            }
            else if (Attribute.IsDefined(implementation, typeof(IgnoreAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(IgnoreAttribute)))
            {
                logger.LogDebug(IgnoreMessage, contractType.FullName, implementation.FullName);
                return serviceDescriptors;
            }

            return serviceDescriptors.Add(contractType, implementation, ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddByAttribute(this IServiceCollection serviceDescriptors, Type contractType, IEnumerable<Type> implementations)
        {
            var noImplementations = true;
            foreach (var implementation in implementations)
            {
                noImplementations = false;
                _ = serviceDescriptors.AddByAttribute(contractType, implementation);
            }

            if (noImplementations)
            {
                logger.LogWarning("No implementation found for {0}", contractType);
            }

            return serviceDescriptors;
        }

        public static IServiceCollection AddFromAssemblies(this IServiceCollection serviceDescriptors, IEnumerable<string> assemblies)
        {
            var loadedTypes = (Assembly.GetEntryAssembly()?.GetTypes() ?? Assembly.GetExecutingAssembly()?.GetTypes() ?? new Type[0])
                .Concat(assemblies.SelectMany(x => Helper.LoadTypesFromAssemblySafe(x, logger)))
                .Where(t => !Attribute.IsDefined(t, typeof(IgnoreAttribute)));

            var interfaces = loadedTypes.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                var implementations = loadedTypes.GetImplementations(@interface);
                _ = serviceDescriptors.AddByAttribute(@interface, implementations);
            }

            return serviceDescriptors;
        }

        public static IServiceCollection AddNonGenericLoggerError(this IServiceCollection serviceDescriptors)
            => serviceDescriptors
                .AddSingleton<ILogger>(t => throw new NotImplementedException($"Don't use {typeof(ILogger)}, use the generic {typeof(ILogger<>)}"));

        public static IServiceCollection AddRegisteredTypes(this IServiceCollection serviceDescriptors)
            => serviceDescriptors
                .TryAddSingleton(serviceDescriptors)
                .TryAddByAttribute(typeof(IRegisteredTypes<>), typeof(RegisteredTypes<>));

        public static IServiceCollection SetLogger(this IServiceCollection serviceDescriptors, ILogger logger)
        {
            IServiceCollectionExtension.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return serviceDescriptors;
        }

        public static IServiceCollection TryAdd(this IServiceCollection serviceDescriptors, Type contractType, Type implementation, ServiceLifetime serviceLifetime)
        {
            if (implementation.IsGenericTypeDefinition)
            {
                logger.LogDebug(GenericAddMessage, contractType.FullName, implementation.FullName, serviceLifetime);
                return serviceDescriptors.TryAdd(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
            }

            logger.LogDebug(AddMessage, contractType.FullName, implementation.FullName, serviceLifetime);

            if (contractType == implementation)
            {
                return serviceDescriptors.TryAdd(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
            }

            return serviceDescriptors
                .TryAdd(ServiceDescriptor.Describe(contractType, x => x.GetService(implementation), serviceLifetime))
                .TryAdd(ServiceDescriptor.Describe(implementation, implementation, serviceLifetime));
        }

        public static IServiceCollection TryAddByAttribute(this IServiceCollection serviceDescriptors, Type contractType, Type implementation)
        {
            if (Attribute.IsDefined(implementation, typeof(SingletonAttribute)) ||
                Attribute.IsDefined(contractType, typeof(SingletonAttribute)))
            {
                return serviceDescriptors.TryAdd(contractType, implementation, ServiceLifetime.Singleton);
            }
            else if (Attribute.IsDefined(implementation, typeof(TransientAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(TransientAttribute)))
            {
                return serviceDescriptors.TryAdd(contractType, implementation, ServiceLifetime.Transient);
            }
            else if (Attribute.IsDefined(implementation, typeof(IgnoreAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(IgnoreAttribute)))
            {
                logger.LogDebug(IgnoreMessage, contractType.FullName, implementation.FullName);
                return serviceDescriptors;
            }

            return serviceDescriptors.TryAdd(contractType, implementation, ServiceLifetime.Scoped);
        }
    }
}