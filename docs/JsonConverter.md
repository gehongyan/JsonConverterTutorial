# JSON 序列化的自定义转换器

转换器是一种将对象或值与 JSON 进行相互转换的类，一般用于实现以下几种目标：

1. 重写内置转换器的默认行为。例如，将 `DateTimeOffset` 值与 Unix 时间戳进行相互转换。
2. 为内置转换器不支持的类型提供 JSON 序列化和反序列化的支持。例如，在 .NET 6 中，内置转换器不支持将 `DateOnly` 或 `TimeOnly` 的值与 JSON 进行相互转换。
3. 为自定义类型提供 JSON 序列化和反序列化的支持。例如，将 `Person` 类型与 JSON 进行相互转换。
4. 其它需要自订序列化与反序列化行为的情况。

## JSON 转换器

以重写 `DateTimeOffset` 类型的 JSON 序列化默认行为为例。

`DateTimeOffset` 的默认序列化结果与 `.ToString(@"'""'yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz'""'")` 的结果一致，即将
`DateTimeOffset` 的值转换为 ISO 8601 格式的字符串。

如果我们希望将 `DateTimeOffset` 的值转换为 Unix 时间戳，可以通过重写 `DateTimeOffset` 的默认转换器来实现。

### 转换器的应用

主要有两种常用方法用于注册转换器：

- 通过 `JsonSerializerOptions.Converters` 属性注册转换器。

```csharp
JsonSerializerOptions options = new();
options.Converters.Add(new DateTimeOffsetTimeStampConverter());
JsonSerializer.Serialize(obj, options);
```

- 通过 `JsonConverterAttribute` 特性注册转换器。

```csharp
class Person
{
    [JsonConverter(typeof(DateTimeOffsetTimeStampConverter))]
    public DateTimeOffset BirthDate { get; set; }
}
```

### 转换器的工厂方法

如果转换器需要接受泛型参数，为多种类型提供转换支持，可以通过转换器的工厂方法来实现。

### 对于复杂 API 行为的支持

如有第三方 API 的行为不符合预期，可以通过转换器来实现对其行为的支持。
