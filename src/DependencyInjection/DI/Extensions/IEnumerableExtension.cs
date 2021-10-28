using System.Collections.Generic;

namespace VectronsLibrary.DI.Extensions;

/// <summary>
/// Extensions for <see cref="IEnumerable{Type}"/>.
/// </summary>
internal static class IEnumerableExtension
{
    /// <summary>
    /// Force collection to not have null values.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to remove null items from.</param>
    /// <returns>The source <see cref="IEnumerable{T}"/> without <c>null</c> values.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        where T : class
    {
        foreach (var t in enumerable)
        {
            if (t != null)
            {
                yield return t;
            }
        }
    }
}