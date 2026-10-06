using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogicBuilder.App.Utils.Json
{
    public class ObjectConverter : JsonConverter<object>
    {
        private readonly IReadOnlyDictionary<string, Type> knownTypes;
        private readonly IKnownTypeNameHelper knownTypeNameHelper;
        private readonly IKnownTypeHelper knownTypeHelper;

        private static readonly HashSet<string> KnownTypeStrings = ["typefullname", "typestring"];

        public ObjectConverter(params Assembly[] assemblies)
            : this((IEnumerable<Assembly>)assemblies)
        {
        }

        protected ObjectConverter(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            knownTypeNameHelper = new KnownTypeNameHelper();
            knownTypeHelper = new KnownTypeHelper(knownTypeNameHelper);

            Assembly[] assemblyList = [.. assemblies.Where(a => a != null).Distinct()];

            knownTypes = knownTypeHelper.BuildKnownTypes(assemblyList.SelectMany(knownTypeHelper.LoadTypesFromAssembly));
        }

        protected ObjectConverter(IEnumerable<Type> allowedTypes)
        {
            if (allowedTypes == null)
                throw new ArgumentNullException(nameof(allowedTypes));

            knownTypeNameHelper = new KnownTypeNameHelper();
            knownTypeHelper = new KnownTypeHelper(knownTypeNameHelper);

            Type[] typeList = [.. allowedTypes];
            Type? disallowedType = typeList.FirstOrDefault(type => !knownTypeHelper.IsAllowedType(type));
            if (disallowedType != null)
                throw new ArgumentException($"{disallowedType.FullName} must be a concrete type assignable to {typeof(object).FullName}.", nameof(allowedTypes));

            knownTypes = knownTypeHelper.BuildKnownTypes(typeList);
        }

        public override bool CanConvert(Type typeToConvert)
            => typeToConvert == typeof(object);

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.StartObject:
                    return GetObject(ref reader, options);
                case JsonTokenType.None:
                case JsonTokenType.EndObject:
                case JsonTokenType.StartArray:
                case JsonTokenType.EndArray:
                case JsonTokenType.PropertyName:
                case JsonTokenType.Comment:
                    throw new JsonException();
                case JsonTokenType.Number:
                    if (reader.TryGetInt32(out int intValue))
                        return intValue;
                    else if (reader.TryGetInt64(out long longValue))
                        return longValue;
                    else if (reader.TryGetDouble(out double doubleValue))
                        return doubleValue;

                    return 0;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Null:
                    return null;
            }
            return null;
        }

        private object? GetObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonProperty jsonProperty = GetJsonProperty();
            if (!jsonProperty.Equals(default(JsonProperty)))
            {
                return DeserializeKnownType(jsonDocument, jsonProperty, options);
            }

            JsonElement.ObjectEnumerator objectEnumerator = jsonDocument.RootElement.EnumerateObject();
            Dictionary<string, Type> types = [];
            foreach (var obj in objectEnumerator)
            {
                types.Add(obj.Name, MapValueKind(obj.Value));
            }

            return JsonSerializer.Deserialize
            (
                jsonDocument.RootElement.GetRawText(),
                AnonymousTypeFactory.CreateAnonymousType(types)
            );

            JsonProperty GetJsonProperty()
                => jsonDocument.RootElement.EnumerateObject().FirstOrDefault
                (
                    e => KnownTypeStrings.Contains(e.Name.ToLowerInvariant())
                );

            Type MapValueKind(JsonElement jsonElement)
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.Undefined:
                    case JsonValueKind.Object:
                    case JsonValueKind.Array:
                        return typeof(object);
                    case JsonValueKind.String:
                        return typeof(string);
                    case JsonValueKind.Number:
                        if (jsonElement.TryGetInt32(out int _))
                            return typeof(int);
                        else if (jsonElement.TryGetInt64(out long _))
                            return typeof(long);
                        else if (jsonElement.TryGetDouble(out double _))
                            return typeof(double);

                        return typeof(double);
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        return typeof(bool);
                    case JsonValueKind.Null:
                    default:
                        return typeof(object);
                }
            }
        }

        private object? DeserializeKnownType(JsonDocument jsonDocument, JsonProperty jsonProperty, JsonSerializerOptions options)
        {
            if (jsonProperty.Value.ValueKind != JsonValueKind.String)
                throw new JsonException($"The property, {string.Join(" or ", KnownTypeStrings)}, must be a string.");

            string objectType = jsonProperty.Value.GetString()!;//does not return null if JsonValueKind is not JsonValueKind.Null

            Type type = knownTypeHelper.ResolveType(objectType, knownTypes)
                ?? throw new JsonException($"Type \"{objectType}\" is not an allowed type for {typeof(object).FullName}.");

            return jsonDocument.RootElement.Deserialize(type, options)!;//never null because only valid JSON like "null" can return null.  For this method, the JsonTokenType is always JsonTokenType.StartObject.
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            Type type = value.GetType();
            if (type == typeof(string) 
                || type == typeof(bool) 
                || IsNumericType(type) 
                || IsAnonymousType(type))
            {
                JsonSerializer.Serialize(writer, value, type, options);
                return;
            }

            Type lookupType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (!knownTypes.TryGetValue(knownTypeNameHelper.GetKey(lookupType), out Type? knownType) || knownType != lookupType)
                throw new JsonException($"Type \"{type.AssemblyQualifiedName}\" is not an allowed type for {typeof(object).FullName}.");

            JsonSerializer.Serialize(writer, value, type, options);
        }

        private static bool IsAnonymousType(Type type)
            => type.Name.Contains("AnonymousType")
            &&
            (
                Attribute.IsDefined
                (
                    type,
                    typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    false
                )
                ||
                type.Assembly.IsDynamic
            );

        private static bool IsNumericType(Type type)
        {
            return Type.GetTypeCode(type) switch
            {
                TypeCode.Byte or TypeCode.SByte or TypeCode.UInt16 or TypeCode.Int16 or
                TypeCode.UInt32 or TypeCode.Int32 or TypeCode.UInt64 or TypeCode.Int64 or
                TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
                _ => false
            };
        }
    }
}
