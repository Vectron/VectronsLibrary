using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using VectronsLibrary.DI.Attributes;
using VectronsLibrary.DI.Extensions;

namespace VectronsLibrary.DI;

/// <summary>
/// The default implementation of <see cref="IRegisteredTypes{T}"/>.
/// </summary>
/// <typeparam name="T">The base type to look for.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="RegisteredTypes{T}"/> class.
/// </remarks>
/// <param name="serviceCollection">The <see cref="IServiceCollection"/> used to built the <see cref="IServiceProvider"/>.</param>
[Singleton]
public class RegisteredTypes<T>(IServiceCollection serviceCollection) : IRegisteredTypes<T>
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0032:Use auto property", Justification = "Auto property will fight with formatting")]
    private readonly IEnumerable<Type> items = serviceCollection
        .Where(x => x.ServiceType == typeof(T) || x.ServiceType.GetInterfaces().Contains(typeof(T)))
        .Select(x => x.ImplementationType)
        .WhereNotNull()
        .Distinct();

    /// <inheritdoc />
    public IEnumerable<Type> Items => items;
}
