using System.Text.Json.Serialization;

namespace TTC.Core;

public abstract class Rule
{
    [JsonIgnore] public string? Name { get; set; }
    public abstract void Apply(TimeTableWave wave);
}
