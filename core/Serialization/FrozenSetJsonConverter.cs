using System.Text.Json;
using System.Text.Json.Serialization;

namespace TTC.Console;

public sealed class FrozenSetJsonConverter<T> : JsonConverter<FrozenSet<T>>
{
    public override FrozenSet<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = JsonSerializer.Deserialize<IEnumerable<T>>(ref reader, options);
        return list?.ToFrozenSet() ?? throw new JsonException("Unable to convert JSON array to FrozenSet");
    }

    public override void Write(Utf8JsonWriter writer, FrozenSet<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.AsEnumerable(), options);
    }
}
