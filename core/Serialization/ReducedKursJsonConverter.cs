using System.Text.Json;
using System.Text.Json.Serialization;

namespace TTC.Console;

public sealed class ReducedKursJsonConverter(IEnumerable<Kurs> kurse) : JsonConverter<Kurs>
{
    public FrozenDictionary<string, Kurs> Kurse { get; } = kurse.ToFrozenDictionary(k => k.Slug);

    public override Kurs? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Kurse[reader.GetString() ?? throw new JsonException()];
    }

    public override void Write(Utf8JsonWriter writer, Kurs value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Slug);
    }
}
