using System;
using System.Collections.Generic;
using System.Text;

namespace VectronsLibrary.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class IEnumerableExtension
{
    /// <summary>
    /// Do a certain action on every item in the collection. (this will iterate the collection).
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    /// <param name="source">A <see cref="IEnumerable{T}"/> to apply the action on.</param>
    /// <param name="action">The <see cref="Action"/> to apply on every item.</param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        source.ThrowIfNull(nameof(source));
        action.ThrowIfNull(nameof(action));
        foreach (var element in source)
        {
            action(element);
        }
    }

    /// <summary>
    /// Converts a <see cref="IEnumerable{T}"/> to a string with each value separated by a ",".
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    /// <param name="items">The <see cref="IEnumerable{T}"/> to turn into a CSV string.</param>
    /// <returns>A string with all values separated by ",".</returns>
    [Obsolete("Method is deprecated please use string.Join()'")]
    public static string ToCSV<T>(this IEnumerable<T> items)
        => items.ToCSV(',');

    /// <summary>
    /// Converts a <see cref="IEnumerable{T}"/> to a string with each value separated by the given separated.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    /// <param name="items">The <see cref="IEnumerable{T}"/> to turn into a CSV string.</param>
    /// <param name="separator">the string to separated the values.</param>
    /// <returns>A string with all values separated by the given separated.</returns>
    [Obsolete("Method is deprecated please use string.Join()")]
    public static string ToCSV<T>(this IEnumerable<T> items, char separator)
    {
        var builder = new StringBuilder();

        foreach (var item in items)
        {
            _ = builder.Append(item);
            _ = builder.Append(separator);
        }

        return builder.ToString().TrimEnd(separator);
    }
}
