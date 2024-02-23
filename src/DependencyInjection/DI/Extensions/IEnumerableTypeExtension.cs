using System;
using System.Collections.Generic;
using System.Linq;
using VectronsLibrary.DI.Attributes;

namespace VectronsLibrary.DI.Extensions;

/// <summary>
/// Extensions for <see cref="IEnumerable{Type}"/>.
/// </summary>
internal static class IEnumerableTypeExtension
{
    /// <summary>
    /// Get all implementations of a specific type.
    /// </summary>
    /// <param name="loadedTypes">An <see cref="IEnumerable{Type}"/> with a loaded types in the domain.</param>
    /// <param name="contractType">The base type that needs to be inherited.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> with all found implementations.</returns>
    public static IEnumerable<Type> GetImplementations(this IEnumerable<Type> loadedTypes, Type contractType)
        => loadedTypes
            .Where(t =>
            !Attribute.IsDefined(t, typeof(IgnoreAttribute))
            && !t.IsInterface
            && !t.IsAbstract
            && contractType.IsAssignableFrom(t)
            && !t.IsGenericTypeDefinition);

    /// <summary>
    /// Gets all interfaces off a type.
    /// </summary>
    /// <param name="loadedTypes">The <see cref="Type"/> to get all interfaces from.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> with all found interfaces.</returns>
    public static IEnumerable<Type> GetInterfaces(this IEnumerable<Type> loadedTypes)
        => loadedTypes
            .Where(t => t.IsInterface)
            .Union(loadedTypes.SelectMany(t => t.GetInterfaces()))
            .Where(c => !Attribute.IsDefined(c, typeof(IgnoreAttribute))
            && !c.IsGenericTypeDefinition
            && !string.IsNullOrWhiteSpace(c.FullName)
            && !c.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
            .Distinct()
            .OrderBy(c => c.FullName);
}
