using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using VectronsLibrary.DI.Attributes;
using VectronsLibrary.DI.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace VectronsLibrary.DI;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static partial class IServiceCollectionExtension
{
    private static ILogger logger = NullLogger.Instance;

    /// <summary>
    /// Adds an item to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the item to.</param>
    /// <param name="contractType">The type of the service. </param>
    /// <param name="implementation">The type of the implementation.</param>
    /// <param name="serviceLifetime">The lifetime of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection Add(this IServiceCollection serviceDescriptors, Type contractType, Type implementation, ServiceLifetime serviceLifetime)
    {
        if (implementation.IsGenericTypeDefinition)
        {
            logger.LogAddingGenericType(contractType.FullName, implementation.FullName, serviceLifetime);
            serviceDescriptors.Add(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
            return serviceDescriptors;
        }

        logger.LogAddingType(contractType.FullName, implementation.FullName, serviceLifetime);

        if (contractType == implementation)
        {
            serviceDescriptors.Add(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
            return serviceDescriptors;
        }

        serviceDescriptors.Add(ServiceDescriptor.Describe(contractType, x => x.GetRequiredService(implementation), serviceLifetime));
        serviceDescriptors.Add(ServiceDescriptor.Describe(implementation, implementation, serviceLifetime));
        return serviceDescriptors;
    }

    /// <summary>
    /// Add the default <see cref="IAssemblyResolver"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddAssemblyResolver(this IServiceCollection serviceDescriptors)
        => serviceDescriptors.TryAddByAttribute(typeof(IAssemblyResolver), typeof(AssemblyResolver));

    /// <summary>
    /// Add a service to the <see cref="IServiceCollection"/> by attribute.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="contractType">The type of the service to register.</param>
    /// <param name="implementation">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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
            logger.LogIgnoreType(contractType.FullName, implementation.FullName);
            return serviceDescriptors;
        }

        return serviceDescriptors.Add(contractType, implementation, ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Add a service to the <see cref="IServiceCollection"/> by attribute.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="contractType">The type of the service to register.</param>
    /// <param name="implementations">The implementations of the service type.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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
            logger.LogNoImplementation(contractType.FullName);
        }

        return serviceDescriptors;
    }

    /// <summary>
    /// Add all types from a assembly to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="assemblies">A <see cref="IEnumerable{T}"/> of assembly names to load types from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddFromAssemblies(this IServiceCollection serviceDescriptors, IEnumerable<string> assemblies)
    {
        var loadedTypes = (Assembly.GetEntryAssembly()?.GetTypes() ?? Assembly.GetExecutingAssembly()?.GetTypes() ?? [])
            .Concat(assemblies.SelectMany(x => AssemblyTypeLoader.LoadTypesFromAssemblySafe(x, logger)))
            .Where(t => !Attribute.IsDefined(t, typeof(IgnoreAttribute)));

        var interfaces = loadedTypes.GetInterfaces();

        foreach (var @interface in interfaces)
        {
            var implementations = loadedTypes.GetImplementations(@interface);
            _ = serviceDescriptors.AddByAttribute(@interface, implementations);
        }

        return serviceDescriptors;
    }

    /// <summary>
    /// Add a error when a class asks for the <see cref="ILogger"/> instead of <see cref="ILogger{TCategoryName}"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddNonGenericLoggerError(this IServiceCollection serviceDescriptors)
        => serviceDescriptors
            .AddSingleton<ILogger>(t => throw new NotSupportedException($"Don't use {typeof(ILogger)}, use the generic {typeof(ILogger<>)}"));

    /// <summary>
    /// Add <see cref="IRegisteredTypes{T}"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddRegisteredTypes(this IServiceCollection serviceDescriptors)
        => serviceDescriptors
            .TryAddSingleton(serviceDescriptors)
            .TryAddByAttribute(typeof(IRegisteredTypes<>), typeof(RegisteredTypes<>));

    /// <summary>
    /// Set the logger to use by the extension methods.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="logger">The <see cref="ILogger"/> instance to log to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection SetLogger(this IServiceCollection serviceDescriptors, ILogger logger)
    {
        IServiceCollectionExtension.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        return serviceDescriptors;
    }

    /// <summary>
    /// Try to add an item to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the item to.</param>
    /// <param name="contractType">The type of the service. </param>
    /// <param name="implementation">The type of the implementation.</param>
    /// <param name="serviceLifetime">The lifetime of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection TryAdd(this IServiceCollection serviceDescriptors, Type contractType, Type implementation, ServiceLifetime serviceLifetime)
    {
        if (implementation.IsGenericTypeDefinition)
        {
            logger.LogAddingGenericType(contractType.FullName, implementation.FullName, serviceLifetime);
            return serviceDescriptors.TryAdd(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime));
        }

        logger.LogAddingType(contractType.FullName, implementation.FullName, serviceLifetime);

        return contractType == implementation
            ? serviceDescriptors.TryAdd(ServiceDescriptor.Describe(contractType, implementation, serviceLifetime))
            : serviceDescriptors
                .TryAdd(ServiceDescriptor.Describe(contractType, x => x.GetRequiredService(implementation), serviceLifetime))
                .TryAdd(ServiceDescriptor.Describe(implementation, implementation, serviceLifetime));
    }

    /// <summary>
    /// Try to add an item by attribute to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to add the item to.</param>
    /// <param name="contractType">The type of the service. </param>
    /// <param name="implementation">The type of the implementation.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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
            logger.LogIgnoreType(contractType.FullName, implementation.FullName);
            return serviceDescriptors;
        }

        return serviceDescriptors.TryAdd(contractType, implementation, ServiceLifetime.Scoped);
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Adding contract type: {ContractType}, with generic implementation type: {ImplementationType} as {Lifetime}")]
    private static partial void LogAddingGenericType(this ILogger logger, string? contractType, string? implementationType, ServiceLifetime lifetime);

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Adding contract type: {ContractType}, with implementation type: {ImplementationType} as {Lifetime}")]
    private static partial void LogAddingType(this ILogger logger, string? contractType, string? implementationType, ServiceLifetime lifetime);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Ignoring contract type: {ContractType}, with implementation type: {ImplementationType}")]
    private static partial void LogIgnoreType(this ILogger logger, string? contractType, string? implementationType);

    [LoggerMessage(
    EventId = 3,
    Level = LogLevel.Warning,
    Message = "No implementation found for {ContractType}")]
    private static partial void LogNoImplementation(this ILogger logger, string? contractType);
}
