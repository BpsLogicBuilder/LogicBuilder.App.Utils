using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogicBuilder.App.Utils.Json
{
    internal interface IKnownTypeHelper
    {
        /// <summary>
        /// Builds the dictionary of allowed types
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, Type> BuildKnownTypes(IEnumerable<Type> types);

        /// <summary>
        /// Allowed if the type is a concrete type assignable to object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsAllowedType(Type? type);

        IEnumerable<Type> LoadTypesFromAssembly(Assembly assembly);
        Type? ResolveType(string typeString, IReadOnlyDictionary<string, Type> knownTypes);
    }
}
