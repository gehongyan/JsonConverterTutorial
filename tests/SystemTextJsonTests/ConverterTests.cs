using System.Drawing;
using SystemTextJsonTests.Models;
using Xunit.Abstractions;

namespace SystemTextJsonTests;

public class ConverterTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ConverterTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void DateTimeOffsetDefaultBehaviorsTest()
    {
        DateTimeOffset now = DateTimeOffset.Now;
        string json = JsonSerializer.Serialize(new DataWrapper<DateTimeOffset>(now));
        string expected = @$"{{""Data"":""{now.ToString(@"yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz")}""}}";
        _testOutputHelper.WriteLine(json);
        Assert.Equal(expected, json);

        DataWrapper<DateTimeOffset>? result = JsonSerializer.Deserialize<DataWrapper<DateTimeOffset>>(json);
        Assert.Equal(now, result?.Data);
    }

    [Fact]
    public void CustomTimestampConverterTest()
    {
        JsonSerializerOptions options = new();
        options.Converters.Add(new DateTimeOffsetTimestampConverter());
        DateTimeOffset time = new(2023, 5, 16, 16, 30, 45, 678, TimeSpan.Zero);
        string json = JsonSerializer.Serialize(new DataWrapper<DateTimeOffset>(time), options);
        string expected = @$"{{""Data"":{time.ToUnixTimeMilliseconds().ToString()}}}";
        _testOutputHelper.WriteLine(json);
        Assert.Equal(expected, json);

        DataWrapper<DateTimeOffset>? result = JsonSerializer.Deserialize<DataWrapper<DateTimeOffset>>(json, options);
        Assert.Equal(time, result?.Data);
    }

    [Fact]
    public void JsonConverterAttributeTest()
    {
        Person person = new(new DateTimeOffset(2023, 5, 16, 16, 30, 45, 678, TimeSpan.Zero));
        string json = JsonSerializer.Serialize(person);
        string expected = $@"{{""BirthDate"":{person.BirthDate.ToUnixTimeMilliseconds()}}}";
        _testOutputHelper.WriteLine(json);
        Assert.Equal(expected, json);

        Person? result = JsonSerializer.Deserialize<Person>(json);
        Assert.Equal(person.BirthDate, result?.BirthDate);
    }

    [Fact]
    public void JsonConverterFactoryTest()
    {
        DateTimeOffset dateTimeOffset = new(2023, 5, 16, 16, 30, 45, 678, TimeSpan.Zero);
        DateTime dateTime = dateTimeOffset.DateTime;
        Dates dates = new(dateTime, null, dateTimeOffset, dateTimeOffset);
        JsonSerializerOptions options = new() { WriteIndented = true };
        options.Converters.Add(new DateTimeConverterFactory());
        string json = JsonSerializer.Serialize(dates, options);

        _testOutputHelper.WriteLine(json);
        const string expected = """
            {
              "DateTime": 1684254645678,
              "DateTimeNullable": null,
              "DateTimeOffset": 1684254645678,
              "DateTimeOffsetNullable": 1684254645678
            }
            """;
        Assert.Equal(expected, json);
    }

    [Fact]
    public void NumberBooleanConverterTest()
    {
        JsonSerializerOptions options = new();
        options.Converters.Add(new NumberBooleanConverter());

        bool result = JsonSerializer.Deserialize<bool>("true", options);
        Assert.True(result);
        result = JsonSerializer.Deserialize<bool>("false", options);
        Assert.False(result);

        result = JsonSerializer.Deserialize<bool>("1", options);
        Assert.True(result);
        result = JsonSerializer.Deserialize<bool>("0", options);
        Assert.False(result);

        result = JsonSerializer.Deserialize<bool>("\"1\"", options);
        Assert.True(result);
        result = JsonSerializer.Deserialize<bool>("\"0\"", options);
        Assert.False(result);
    }

    [Fact]
    public void GradiantColorConverterTest()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Converters = { new HexColorConverter(), new NullableGradientColorConverter() }
        };
        List<GradientColor?> gradientColors = new()
        {
            new GradientColor(Color.Aqua, Color.Chartreuse),
            new GradientColor(Color.Orange, Color.Moccasin),
            null
        };
        string json = JsonSerializer.Serialize(gradientColors, options);
        _testOutputHelper.WriteLine(json);
        const string expected = """
            [
              {
                "color_list": [
                  "#00FFFF",
                  "#7FFF00"
                ]
              },
              {
                "color_list": [
                  "#FFA500",
                  "#FFE4B5"
                ]
              },
              []
            ]
            """;
        Assert.Equal(expected, json);
        List<GradientColor?>? results = JsonSerializer.Deserialize<GradientColor?[]>(json, options)?.ToList();
        Assert.NotNull(results);
        Assert.Equal(gradientColors.Count, results?.Count);
        foreach ((GradientColor? expectedColor, GradientColor? resultColor) in gradientColors.Zip(results!))
        {
            Assert.Equal(expectedColor?.Left.R, resultColor?.Left.R);
            Assert.Equal(expectedColor?.Left.G, resultColor?.Left.G);
            Assert.Equal(expectedColor?.Left.B, resultColor?.Left.B);
            Assert.Equal(expectedColor?.Right.R, resultColor?.Right.R);
            Assert.Equal(expectedColor?.Right.G, resultColor?.Right.G);
            Assert.Equal(expectedColor?.Right.B, resultColor?.Right.B);
        }
    }
}
