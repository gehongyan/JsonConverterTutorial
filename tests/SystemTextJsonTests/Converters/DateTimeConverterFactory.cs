namespace SystemTextJsonTests.Converters;

public class DateTimeConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(DateTime)
        || typeToConvert == typeof(DateTime?)
        || typeToConvert == typeof(DateTimeOffset)
        || typeToConvert == typeof(DateTimeOffset?);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(DateTime))
            return new DateTimeConverter<DateTime>();
        if (typeToConvert == typeof(DateTime?))
            return new DateTimeConverter<DateTime?>();
        if (typeToConvert == typeof(DateTimeOffset))
            return new DateTimeConverter<DateTimeOffset>();
        if (typeToConvert == typeof(DateTimeOffset?))
            return new DateTimeConverter<DateTimeOffset?>();

        throw new NotSupportedException("CreateConverter got called on a type that this converter factory doesn't support");
    }

    private sealed class DateTimeConverter<T> : JsonConverter<T>
    {
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            dynamic? ticks = (value as dynamic)?.Ticks;
            DateTimeOffset dateTimeOffset = new DateTimeOffset(ticks, TimeSpan.Zero);
            writer.WriteNumberValue(dateTimeOffset.ToUnixTimeMilliseconds());
        }
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();
    }
}
