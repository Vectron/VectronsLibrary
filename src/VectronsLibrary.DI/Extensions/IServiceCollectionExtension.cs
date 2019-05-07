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
        private static ILogger logger = NullLogger.Instance;

        public static IServiceCollection AddAssemblyResolver(this IServiceCollection serviceDescriptors)
            => serviceDescriptors
                .TryAddByAttribute(typeof(AssemblyResolver), typeof(IAssemblyResolver));

        public static IServiceCollection AddByAttribute(this IServiceCollection serviceDescriptors, Type implementation, Type contractType)
        {
            if (Attribute.IsDefined(implementation, typeof(SingletonAttribute)) ||
                Attribute.IsDefined(contractType, typeof(SingletonAttribute)))
            {
                if (implementation.IsGenericTypeDefinition)
                {
                    logger.LogDebug("Adding contract type: {0}, with generic implementation type: {1} as singleton",
                        contractType.FullName,
                        implementation.FullName);
                    return serviceDescriptors.AddSingleton(contractType, implementation);
                }

                if (contractType == implementation)
                {
                    logger.LogDebug("Adding contract type: {0}, with implementation type: {1} as singleton",
                            contractType.FullName,
                            implementation.FullName);
                    return serviceDescriptors.AddSingleton(contractType, implementation);
                }

                logger.LogDebug("Adding contract type: {0}, with implementation type: {1} as singleton",
                        contractType.FullName,
                        implementation.FullName);
                return serviceDescriptors.AddSingleton(contractType, x => x.GetService(implementation))
                    .AddSingleton(implementation);
            }
            else if (Attribute.IsDefined(implementation, typeof(TransientAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(TransientAttribute)))
            {
                if (implementation.IsGenericTypeDefinition)
                {
                    logger.LogDebug("Adding contract type: {0}, with generic implementation type: {1} as transient",
                           contractType.FullName,
                           implementation.FullName);
                    return serviceDescriptors.AddTransient(contractType, implementation);
                }

                if (contractType == implementation)
                {
                    logger.LogDebug("Adding contract type: {0}, with implementation type: {1} as transient",
                            contractType.FullName,
                            implementation.FullName);
                    return serviceDescriptors.AddTransient(contractType, implementation);
                }

                logger.LogDebug("Adding contract type: {0}, with implementation type: {1} as transient",
                        contractType.FullName,
                        implementation.FullName);
                return serviceDescriptors.AddTransient(contractType, x => x.GetService(implementation))
                    .AddTransient(implementation);
            }
            else if (Attribute.IsDefined(implementation, typeof(IgnoreAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(IgnoreAttribute)))
            {
                logger.LogDebug("Ignoring contract type: {0}, with implementation type: {1} as scoped",
                       contractType.FullName,
                       implementation.FullName);
                return serviceDescriptors;
            }
            else
            {
                if (implementation.IsGenericTypeDefinition)
                {
                    logger.LogDebug("Adding contract type: {0}, with generic implementation type: {1} as scoped",
                           contractType.FullName,
                           implementation.FullName);
                    return serviceDescriptors.AddScoped(contractType, implementation);
                }

                if (contractType == implementation)
                {
                    logger.LogDebug("Adding contract type: {0}, with implementation type: {1} as scoped",
                            contractType.FullName,
                            implementation.FullName);
                    return serviceDescriptors.AddScoped(contractType, implementation);
                }

                logger.LogDebug("Adding contract type: {0}, with implementation type: {1} as scoped",
                        contractType.FullName,
                        implementation.FullName);
                return serviceDescriptors.AddScoped(contractType, x => x.GetService(implementation))
                    .AddScoped(implementation);
            }
        }

        public static IServiceCollection AddByAttribute(this IServiceCollection serviceDescriptors, IEnumerable<Type> implementations, Type contractType)
        {
            var noImplementations = true;
            foreach (var implementation in implementations)
            {
                noImplementations = false;
                serviceDescriptors.AddByAttribute(implementation, contractType);
            }

            if (noImplementations)
            {
                logger.LogWarning("No implementation found for {0}", contractType);
            }

            return serviceDescriptors;
        }

        public static IServiceCollection AddFromAssemblies(this IServiceCollection serviceDescriptors, IEnumerable<string> assemblies)
        {
            var loadedTypes = (Assembly.GetEntryAssembly()?.GetTypes() ?? new Type[0])
                .Concat(assemblies.SelectMany(x => Helper.LoadTypesFromAssemblySafe(x, logger)))
                .Where(t => !Attribute.IsDefined(t, typeof(IgnoreAttribute)));

            var interfaces = loadedTypes.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                var implementations = loadedTypes.GetImplementations(@interface);
                serviceDescriptors.AddByAttribute(implementations, @interface);
            }

            return serviceDescriptors;
        }

        public static IServiceCollection AddNonGenericLoggerError(this IServiceCollection serviceDescriptors)
            => serviceDescriptors
                .AddSingleton<ILogger>(t => throw new NotImplementedException($"Don't use {typeof(ILogger)}, use the generic {typeof(ILogger<>)}"));

        public static IServiceCollection AddRegisteredTypes(this IServiceCollection serviceDescriptors)
            => serviceDescriptors
                .TryAddSingleton(serviceDescriptors)
                .TryAddByAttribute(typeof(RegisteredTypes<>), typeof(IRegisteredTypes<>));

        public static IServiceCollection SetLogger(this IServiceCollection serviceDescriptors, ILogger logger)
        {
            IServiceCollectionExtension.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddByAttribute(this IServiceCollection serviceDescriptors, Type implementation, Type contractType)
        {
            if (Attribute.IsDefined(implementation, typeof(SingletonAttribute)) ||
                Attribute.IsDefined(contractType, typeof(SingletonAttribute)))
            {
                if (implementation.IsGenericTypeDefinition)
                {
                    logger.LogDebug("Try adding contract type: {0}, with generic implementation type: {1} as singleton",
                        contractType.FullName,
                        implementation.FullName);
                    return serviceDescriptors.TryAddSingleton(contractType, implementation);
                }

                if (contractType == implementation)
                {
                    logger.LogDebug("Try Adding contract type: {0}, with implementation type: {1} as singleton",
                            contractType.FullName,
                            implementation.FullName);
                    return serviceDescriptors.TryAddSingleton(contractType, implementation);
                }

                logger.LogDebug("Try adding contract type: {0}, with implementation type: {1} as singleton",
                        contractType.FullName,
                        implementation.FullName);
                return serviceDescriptors.TryAddSingleton(contractType, x => x.GetService(implementation))
                    .TryAddSingleton(implementation);
            }
            else if (Attribute.IsDefined(implementation, typeof(TransientAttribute)) ||
                     Attribute.IsDefined(contractType, typeof(TransientAttribute)))
            {
                if (implementation.IsGenericTypeDefinition)
                {
                    logger.LogDebug("Try adding contract type: {0}, with generic implementation type: {1} as transient",
                           contractType.FullName,
                           implementation.FullName);
                    return serviceDescriptors.TryAddTransient(contractType, implementation);
                }

                if (contractType == implementation)
                {
                    logger.LogDebug("Try Adding contract type: {0}, with implementation type: {1} as transient",
                            contractType.FullName,
                            implementation.FullName);
                    return serviceDescriptors.TryAddTransient(contractType, implementation);
                }

                logger.LogDebug("Try adding contract type: {0}, with implementation type: {1} as transient",
                        contractType.FullName,
                        implementation.FullName);
                return serviceDescriptors.TryAddTransient(contractType, x => x.GetService(implementation))
                    .TryAddTransient(implementation);
            }
            else
            {
                if (implementation.IsGenericTypeDefinition)
                {
                    logger.LogDebug("Try adding contract type: {0}, with generic implementation type: {1} as scoped",
                           contractType.FullName,
                           implementation.FullName);
                    return serviceDescriptors.TryAddScoped(contractType, implementation);
                }

                if (contractType == implementation)
                {
                    logger.LogDebug("Try Adding contract type: {0}, with implementation type: {1} as scoped",
                            contractType.FullName,
                            implementation.FullName);
                    return serviceDescriptors.TryAddScoped(contractType, implementation);
                }

                logger.LogDebug("Try adding contract type: {0}, with implementation type: {1} as scoped",
                        contractType.FullName,
                        implementation.FullName);
                return serviceDescriptors.TryAddScoped(contractType, x => x.GetService(implementation))
                    .TryAddScoped(implementation);
            }
        }
    }
}