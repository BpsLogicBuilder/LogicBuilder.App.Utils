using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogicBuilder.App.Utils.Json
{
    public class ObjectConverter : JsonConverter<object>
    {
        private static readonly HashSet<string> KnownTypeStrings = ["typefullname", "typestring"];

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

        private static object? GetObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonProperty jsonProperty = GetJsonProperty();
            if (!jsonProperty.Equals(default(JsonProperty)))
            {
                return JsonSerializer.Deserialize
                (
                    jsonDocument.RootElement.GetRawText(),
                    Type.GetType(jsonProperty.Value.GetString() ?? "") ?? throw new InvalidOperationException("Type not found"),
                    options
                );
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

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
