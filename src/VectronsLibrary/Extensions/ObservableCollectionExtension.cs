using System.Collections.Generic;

namespace VectronsLibrary.Extensions
{
    public static class ICollectionExtension
    {
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
}