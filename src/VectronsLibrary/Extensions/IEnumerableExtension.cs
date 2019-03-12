using System;
using System.Collections.Generic;
using System.Text;

namespace VectronsLibrary.Extensions
{
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            source.ThrowIfNull(nameof(source));
            action.ThrowIfNull(nameof(action));
            foreach (T element in source)
            {
                action(element);
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> items)
            => items.ToCSV(',');

        public static string ToCSV<T>(this IEnumerable<T> items, char sepperator)
        {
            var builder = new StringBuilder();

            foreach (var item in items)
            {
                builder.Append(item);
                builder.Append(sepperator);
            }

            return builder.ToString().TrimEnd(sepperator);
        }
    }
}