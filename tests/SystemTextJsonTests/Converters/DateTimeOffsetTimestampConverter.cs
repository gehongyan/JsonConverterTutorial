namespace SystemTextJsonTests.Converters;

public sealed class DateTimeOffsetTimestampConverter : JsonConverter<DateTimeOffset>
{
    /// <inheritdoc />
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
}
