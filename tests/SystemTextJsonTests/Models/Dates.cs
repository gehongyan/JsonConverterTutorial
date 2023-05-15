namespace SystemTextJsonTests.Models;

public sealed record Dates(DateTime DateTime, DateTime? DateTimeNullable,
    DateTimeOffset DateTimeOffset, DateTimeOffset? DateTimeOffsetNullable);
