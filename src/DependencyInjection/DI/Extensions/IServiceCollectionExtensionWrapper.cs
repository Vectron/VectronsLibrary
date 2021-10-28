using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <inheritdoc cref="ServiceCollectionDescriptorExtensions"/>
public static class IServiceCollectionExtensionWrapper
{
    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAdd(IServiceCollection, IEnumerable{ServiceDescriptor})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAdd(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
    {
        ServiceCollectionDescriptorExtensions.TryAdd(collection, descriptors);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAdd(IServiceCollection, ServiceDescriptor)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        ServiceCollectionDescriptorExtensions.TryAdd(collection, descriptor);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddEnumerable(IServiceCollection, IEnumerable{ServiceDescriptor})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddEnumerable(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
    {
        ServiceCollectionDescriptorExtensions.TryAddEnumerable(collection, descriptors);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddEnumerable(IServiceCollection, ServiceDescriptor)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddEnumerable(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        ServiceCollectionDescriptorExtensions.TryAddEnumerable(collection, descriptor);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddScoped{TService}(IServiceCollection, Func{IServiceProvider, TService})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddScoped(collection, implementationFactory);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddScoped{TService, TImplementation}(IServiceCollection)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        ServiceCollectionDescriptorExtensions.TryAddScoped<TService, TImplementation>(collection);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddScoped(IServiceCollection, Type, Func{IServiceProvider, object})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
    {
        ServiceCollectionDescriptorExtensions.TryAddScoped(collection, service, implementationFactory);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddScoped{TService}(IServiceCollection)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddScoped<TService>(this IServiceCollection collection)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddScoped<TService>(collection);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddScoped(IServiceCollection, Type)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddScoped(this IServiceCollection collection, Type service)
    {
        ServiceCollectionDescriptorExtensions.TryAddScoped(collection, service);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddScoped(IServiceCollection, Type, Type)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddScoped(this IServiceCollection collection, Type service, Type implementationType)
    {
        ServiceCollectionDescriptorExtensions.TryAddScoped(collection, service, implementationType);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton(IServiceCollection, Type)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton(this IServiceCollection collection, Type service)
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton(collection, service);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton(IServiceCollection, Type, Type)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton(this IServiceCollection collection, Type service, Type implementationType)
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton(collection, service, implementationType);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton(IServiceCollection, Type, Func{IServiceProvider, object})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton(collection, service, implementationFactory);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton{TService}(IServiceCollection)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection collection)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton<TService>(collection);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton{TService, TImplementation}(IServiceCollection)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton<TService, TImplementation>(collection);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton{TService}(IServiceCollection, TService)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection collection, TService instance)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton(collection, instance);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddSingleton{TService}(IServiceCollection, Func{IServiceProvider, TService})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddSingleton(collection, implementationFactory);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddTransient{TService}(IServiceCollection, Func{IServiceProvider, TService})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddTransient(collection, implementationFactory);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddTransient{TService, TImplementation}(IServiceCollection)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        ServiceCollectionDescriptorExtensions.TryAddTransient<TService, TImplementation>(collection);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddTransient{TService}(IServiceCollection)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddTransient<TService>(this IServiceCollection collection)
        where TService : class
    {
        ServiceCollectionDescriptorExtensions.TryAddTransient<TService>(collection);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddTransient(IServiceCollection, Type, Func{IServiceProvider, object})"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddTransient(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
    {
        ServiceCollectionDescriptorExtensions.TryAddTransient(collection, service, implementationFactory);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddTransient(IServiceCollection, Type, Type)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddTransient(this IServiceCollection collection, Type service, Type implementationType)
    {
        ServiceCollectionDescriptorExtensions.TryAddTransient(collection, service, implementationType);
        return collection;
    }

    /// <inheritdoc cref="ServiceCollectionDescriptorExtensions.TryAddTransient(IServiceCollection, Type)"/>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAddTransient(this IServiceCollection collection, Type service)
    {
        ServiceCollectionDescriptorExtensions.TryAddTransient(collection, service);
        return collection;
    }
}