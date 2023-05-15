namespace SystemTextJsonTests.Models;

public sealed record Person(
    [property: JsonConverter(typeof(DateTimeOffsetTimestampConverter))]
    DateTimeOffset BirthDate);
