using System.Collections.Generic;

namespace VectronsLibrary.Extensions;

/// <summary>
/// Class containing extension methods for <see cref="ICollection{T}"/>.
/// </summary>
public static class ICollectionExtension
{
    /// <summary>
    /// Adds a range of items to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type stored in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to add the items to.</param>
    /// <param name="items">The items to add to the <see cref="ICollection{T}"/>.</param>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        collection.ThrowIfNull(nameof(collection));
        items.ThrowIfNull(nameof(items));
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}
