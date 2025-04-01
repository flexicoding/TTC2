using System.Text.Json;
using System.Text.Json.Serialization;

namespace TTC.Console;

public sealed class EnumJsonConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private static readonly FrozenDictionary<string, T> _registry = Enum.GetValues<T>().ToFrozenDictionary(day => day.ToString());
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return _registry[reader.GetString() ?? throw new JsonException()];
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}