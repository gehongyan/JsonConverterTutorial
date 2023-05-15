namespace SystemTextJsonTests.Converters;

internal class NumberBooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number => reader.TryGetInt32(out int value) && 1.Equals(value),
            JsonTokenType.String => "1".Equals(reader.GetString()),
            _ => throw new JsonException(
                $"{nameof(NumberBooleanConverter)} expects boolean, string or number token, but got {reader.TokenType}")
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteBooleanValue(value);
}
