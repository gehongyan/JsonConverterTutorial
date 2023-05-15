using System.Drawing;
using SystemTextJsonTests.Models;

namespace SystemTextJsonTests.Converters;

internal class NullableGradientColorConverter : JsonConverter<GradientColor?>
{
    /// <inheritdoc />
    public override bool HandleNull => true;

    /// <inheritdoc />
    public override GradientColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects property name token, but got {reader.TokenType}");

            string? propertyName = reader.GetString();
            if (propertyName != "color_list")
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects property name 'color_list', but got {propertyName}");

            reader.Read();
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects start array token, but got {reader.TokenType}");

            reader.Read();
            if (reader.TokenType != JsonTokenType.String || reader.GetString() is not { } leftString)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects string token, but got {reader.TokenType}");
            Color left = JsonSerializer.Deserialize<Color>(@$"""{leftString}""", options);

            reader.Read();
            if (reader.TokenType != JsonTokenType.String || reader.GetString() is not { } rightString)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects string token, but got {reader.TokenType}");
            Color right = JsonSerializer.Deserialize<Color>(@$"""{rightString}""", options);

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects end array token, but got {reader.TokenType}");

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects end object token, but got {reader.TokenType}");

            return new GradientColor(left, right);
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException($"{nameof(NullableGradientColorConverter)} expects end array token, but got {reader.TokenType}");

            return null;
        }

        throw new JsonException(
            $"{nameof(NullableGradientColorConverter)} expects start object or start array token, but got {reader.TokenType}");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GradientColor? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("color_list");
            writer.WriteStartArray();
            writer.WriteStringValue($"#{value.Value.Left.R:X2}{value.Value.Left.G:X2}{value.Value.Left.B:X2}");
            writer.WriteStringValue($"#{value.Value.Right.R:X2}{value.Value.Right.G:X2}{value.Value.Right.B:X2}");
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
    }
}
