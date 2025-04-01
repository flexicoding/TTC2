using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TTC.Core.Serialization;

public sealed class RuleJsonConverter(IEnumerable<Type> rules) : JsonConverter<Rule>
{
    private const string TypePropertyName = "$type";
    private readonly FrozenDictionary<string, Type> rules = rules.ToFrozenDictionary(t => t.Name);

    public override Rule? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonNode.Parse(ref reader) as JsonObject ?? throw new JsonException();

        if (!element.TryGetPropertyValue(TypePropertyName, out JsonNode? typeNode) || typeNode?.GetValue<string>() is not string typeName)
        {
            throw new JsonException($"Missing {TypePropertyName} property");
        }

        if (!rules.TryGetValue(typeName, out var type))
        {
            throw new JsonException($"Unkown Rule: {TypePropertyName}");
        }

        return element.Deserialize(type, options) as Rule;

    }

    public override void Write(Utf8JsonWriter writer, Rule value, JsonSerializerOptions options)
    {
        var typeName = value.GetType().Name;
        if (!rules.TryGetValue(typeName, out var type))
        {
            throw new JsonException($"Unkown rule type {typeName}");
        }

        var element = JsonSerializer.SerializeToNode(value, type, options) ?? throw new JsonException();
        element[TypePropertyName] = typeName;
        element.WriteTo(writer);
    }
}
