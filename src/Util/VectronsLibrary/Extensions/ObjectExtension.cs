using System;

namespace VectronsLibrary.Extensions;

/// <summary>
/// Extension methods for <see cref="object"/>.
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    /// Throw a <see cref="ArgumentNullException"/> when obj is null.
    /// </summary>
    /// <typeparam name="T">Type of the object to check.</typeparam>
    /// <param name="obj">The object to check.</param>
    /// <param name="parameterName">The name of the parameter to check.</param>
    public static void ThrowIfNull<T>(this T obj, string parameterName)
        where T : class
    {
        if (obj == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    /// <summary>
    /// Throw a <see cref="ArgumentNullException"/> when obj is null.
    /// </summary>
    /// <typeparam name="T">Type of the object to check.</typeparam>
    /// <param name="obj">The object to check.</param>
    /// <param name="parameterName">The name of the parameter to check.</param>
    public static void ThrowIfNull<T>(this T? obj, string parameterName)
        where T : struct
    {
        if (obj == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}