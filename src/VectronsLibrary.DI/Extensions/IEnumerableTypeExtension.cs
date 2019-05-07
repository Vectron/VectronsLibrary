using System.Linq;
using VectronsLibrary.DI;

namespace System.Collections.Generic
{
    internal static class IEnumerableTypeExtension
    {
        public static IEnumerable<Type> GetImplementations(this IEnumerable<Type> loadedTypes, Type contractType)
            => loadedTypes
                .Where(t =>
                    !Attribute.IsDefined(t, typeof(IgnoreAttribute))
                    && !t.IsInterface
                    && !t.IsAbstract
                    && contractType.IsAssignableFrom(t)
                    && !t.IsGenericTypeDefinition);

        public static IEnumerable<Type> GetInterfaces(this IEnumerable<Type> loadedTypes)
            => loadedTypes
                .Where(t => t.IsInterface)
                .Union(loadedTypes.SelectMany(t => t.GetInterfaces()))
                .Where(c => !Attribute.IsDefined(c, typeof(IgnoreAttribute))
                            && !c.IsGenericTypeDefinition
                            && !string.IsNullOrWhiteSpace(c.FullName)
                            && !c.FullName.StartsWith("System."))
                .Distinct()
                .OrderBy(c => c.FullName);
    }
}