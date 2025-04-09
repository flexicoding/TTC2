using System.Text.Json;
using System.Text.Json.Serialization;

namespace TTC.Core.Serialization;

public sealed class ReducedCoursesJsonConverter(IEnumerable<Course> kurse) : JsonConverter<Course>
{
    public FrozenDictionary<string, Course> Courses { get; } = kurse.ToFrozenDictionary(k => k.Slug);

    public override Course? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Courses[reader.GetString() ?? throw new JsonException()];
    }

    public override void Write(Utf8JsonWriter writer, Course value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Slug);
    }
}
