using System;

namespace VectronsLibrary.Extensions
{
    public static class ObjectExtension
    {
        public static void ThrowIfNull<T>(this T obj, string parameterName)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ThrowIfNull<T>(this T? obj, string parameterName)
            where T : struct
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}