using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicBuilder.App.Utils
{
    public class TypeHelper(ILogger<TypeHelper> logger) : ITypeHelper
    {
        private readonly ILogger<TypeHelper> _logger = logger;

        public object GetPropertyValue(object item, string propertyName)
        {
            try
            {
                return item.GetType().GetProperty
                (
                    propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                )
                .GetValue(item);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "{ExceptionType} : {ExceptionMessage}", ex.GetType().Name, ex.Message);
                throw new InvalidOperationException($"Failed to get property '{propertyName}' from type '{item?.GetType().Name}'.", ex);
            }
        }

        public Type GetType([ParameterEditorControl(ParameterControlType.TypeAutoComplete)] string assemblyQualifiedTypeName)
            => Type.GetType(assemblyQualifiedTypeName);

        public string ToTypeString(Type type) => type.AssemblyQualifiedName;

        public bool TryParse(string toParse, Type type, out object? result)
        {
            if (type == null)
                throw new ArgumentException("Argument cannot be null.", nameof(type));

            if (!IsLiteralType(type))
                throw new ArgumentException("Not a valid literal type.", nameof(type));

            if (type == typeof(string))
            {
                result = toParse;
                return true;
            }

            if (typeof(Enum).IsAssignableFrom(type))
            {
                if (!int.TryParse(toParse, out int _) && !Enum.IsDefined(type, toParse))
                {
                    result = null;
                    return false;
                }

                result = Enum.Parse(type, toParse);
                return true;
            }

            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type);

            MethodInfo method = type.GetMethods().Single(IsTryParseMethod);

            object?[] args = [toParse, null];
            bool success = (bool)method.Invoke(null, args);
            result = success ? args[1] : null;

            return success;

            bool IsTryParseMethod(MethodInfo method)
            {
                if (method.Name != "TryParse") return false;
                ParameterInfo[] parameters = method.GetParameters();
                return parameters.Length == 2
                    && parameters[0].ParameterType == typeof(string)
                    && parameters[1].IsOut
                    && parameters[1].ParameterType.GetElementType() == type;
            }
        }

        private static bool IsNullable(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

        private static bool IsLiteralType(Type type)
        {
            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type);

            return LiteralTypes.Contains(type)
                || UneferencedLiteralTypes.Contains(type.FullName)
                || typeof(Enum).IsAssignableFrom(type);
        }

        private static HashSet<Type> LiteralTypes => [
                typeof(bool),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid),
                typeof(decimal),
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(char),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(string)
            ];

        private static readonly HashSet<string> UneferencedLiteralTypes =
        [
            UnreferencedLiteralTypeNames.DATEONLY,
            UnreferencedLiteralTypeNames.TIMEONLY,
            UnreferencedLiteralTypeNames.DATE,
            UnreferencedLiteralTypeNames.TIMEOFDAY
        ];

        private struct UnreferencedLiteralTypeNames
        {
            public const string DATEONLY = "System.DateOnly";
            public const string TIMEONLY = "System.TimeOnly";
            public const string DATE = "Microsoft.OData.Edm.Date";
            public const string TIMEOFDAY = "Microsoft.OData.Edm.TimeOfDay";
        }
    }
}
