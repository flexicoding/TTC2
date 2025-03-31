using System.Text.Json;
using System.Text.Json.Serialization;

namespace TTC.Core;

public sealed record Person(string ID);

public sealed class PersonJsonConverter : JsonConverter<Person>
{
    public override Person? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() is string id ? new(id) : null;

    public override void Write(Utf8JsonWriter writer, Person value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ID);
    }
}