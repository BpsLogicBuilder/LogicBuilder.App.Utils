using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicBuilder.App.Utils.Json
{
    internal class KnownTypeHelper(IKnownTypeNameHelper knownTypeNameHelper) : IKnownTypeHelper
    {
        private readonly IKnownTypeNameHelper knownTypeNameHelper = knownTypeNameHelper;

        public IReadOnlyDictionary<string, Type> BuildKnownTypes(IEnumerable<Type> types)
        {
            return types.Where(t => IsAllowedType(t)).Aggregate
            (
                new Dictionary<string, Type>(StringComparer.Ordinal),
                (dictionary, type) =>
                {
                    dictionary[knownTypeNameHelper.GetKey(type)] = type;
                    return dictionary;
                }
            );
        }

        public bool IsAllowedType(Type? type)
        {
            return type != null
                && !type.IsAbstract
                && !type.IsInterface
                && (
                        typeof(object).IsAssignableFrom(type)
                        ||
                        (type.IsGenericType && typeof(object).IsAssignableFrom(type.GetGenericTypeDefinition()))
                );
        }

        public IEnumerable<Type> LoadTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null)!;
            }
        }

        public Type? ResolveType(string typeString, IReadOnlyDictionary<string, Type> knownTypes)
        {
            string? key = knownTypeNameHelper.GetKey(typeString);
            return key != null && knownTypes.TryGetValue(key, out Type? _) ? Type.GetType(typeString) : null;
        }
    }
}
