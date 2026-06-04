using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Handles OAuth-style contracts that inconsistently emit either a single string or an array of strings.
    ///
    /// We need this specifically for token introspection because Duende returns <c>aud</c> as an array when
    /// a reference token targets multiple resources, while older callers and some providers still emit one string.
    /// Accepting both shapes here keeps the shared DTO stable instead of forcing each consumer to special-case it.
    /// </summary>
    public sealed class StringOrStringArrayJsonConverter : JsonConverter<string[]>
    {
        public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return Array.Empty<string>();

                case JsonTokenType.String:
                    var singleValue = reader.GetString();
                    return string.IsNullOrWhiteSpace(singleValue) ? Array.Empty<string>() : new[] { singleValue };

                case JsonTokenType.StartArray:
                    var values = new List<string>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                            return values.ToArray();

                        if (reader.TokenType == JsonTokenType.Null)
                            continue;

                        if (reader.TokenType != JsonTokenType.String)
                            throw new JsonException();

                        var item = reader.GetString();
                        if (!string.IsNullOrWhiteSpace(item))
                            values.Add(item);
                    }

                    throw new JsonException();

                default:
                    throw new JsonException();
            }
        }

        public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
        {
            if (value == null || value.Length == 0)
            {
                writer.WriteNullValue();
                return;
            }

            if (value.Length == 1)
            {
                writer.WriteStringValue(value[0]);
                return;
            }

            writer.WriteStartArray();
            foreach (var item in value)
            {
                writer.WriteStringValue(item);
            }
            writer.WriteEndArray();
        }
    }
}
