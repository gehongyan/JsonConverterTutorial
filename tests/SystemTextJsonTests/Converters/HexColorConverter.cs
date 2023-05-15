using System.Drawing;

namespace SystemTextJsonTests.Converters;

internal class HexColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? hex = reader.GetString()?.TrimStart('#');
        if (string.IsNullOrWhiteSpace(hex) || hex.Length < 6) return Color.Empty;
        return Color.FromArgb(int.Parse(hex, System.Globalization.NumberStyles.HexNumber));
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) =>
        writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
}
