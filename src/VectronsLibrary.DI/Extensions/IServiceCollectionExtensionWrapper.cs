using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensionWrapper
    {
        public static IServiceCollection TryAdd(this IServiceCollection serviceDescriptors, IEnumerable<ServiceDescriptor> descriptors)
        {
            ServiceCollectionDescriptorExtensions.TryAdd(serviceDescriptors, descriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAdd(this IServiceCollection serviceDescriptors, ServiceDescriptor descriptor)
        {
            ServiceCollectionDescriptorExtensions.TryAdd(serviceDescriptors, descriptor);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddEnumerable(this IServiceCollection serviceDescriptors, IEnumerable<ServiceDescriptor> descriptors)
        {
            ServiceCollectionDescriptorExtensions.TryAddEnumerable(serviceDescriptors, descriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddEnumerable(this IServiceCollection serviceDescriptors, ServiceDescriptor descriptor)
        {
            ServiceCollectionDescriptorExtensions.TryAddEnumerable(serviceDescriptors, descriptor);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddScoped<TService>(this IServiceCollection serviceDescriptors, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped(serviceDescriptors, implementationFactory);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection serviceDescriptors)
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped<TService, TImplementation>(serviceDescriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddScoped(this IServiceCollection serviceDescriptors, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped(serviceDescriptors, service, implementationFactory);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddScoped<TService>(this IServiceCollection serviceDescriptors)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped<TService>(serviceDescriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddScoped(this IServiceCollection serviceDescriptors, Type service)
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped(serviceDescriptors, service);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddScoped(this IServiceCollection serviceDescriptors, Type service, Type implementationType)
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped(serviceDescriptors, service, implementationType);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton(this IServiceCollection serviceDescriptors, Type service)
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton(serviceDescriptors, service);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton(this IServiceCollection serviceDescriptors, Type service, Type implementationType)
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton(serviceDescriptors, service, implementationType);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton(this IServiceCollection serviceDescriptors, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton(serviceDescriptors, service, implementationFactory);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection serviceDescriptors)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton<TService>(serviceDescriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection serviceDescriptors)
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton<TService, TImplementation>(serviceDescriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection serviceDescriptors, TService instance)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton(serviceDescriptors, instance);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection serviceDescriptors, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton(serviceDescriptors, implementationFactory);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddTransient<TService>(this IServiceCollection serviceDescriptors, Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient(serviceDescriptors, implementationFactory);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection serviceDescriptors)
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient<TService, TImplementation>(serviceDescriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddTransient<TService>(this IServiceCollection serviceDescriptors)
            where TService : class
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient<TService>(serviceDescriptors);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddTransient(this IServiceCollection serviceDescriptors, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient(serviceDescriptors, service, implementationFactory);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddTransient(this IServiceCollection serviceDescriptors, Type service, Type implementationType)
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient(serviceDescriptors, service, implementationType);
            return serviceDescriptors;
        }

        public static IServiceCollection TryAddTransient(this IServiceCollection serviceDescriptors, Type service)
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient(serviceDescriptors, service);
            return serviceDescriptors;
        }
    }
}