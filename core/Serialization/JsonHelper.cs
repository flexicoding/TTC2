using System.IO;
using System.Text.Json;

namespace TTC.Core.Serialization;

public sealed class JsonHelper
{
    public JsonSerializerOptions Options { get; set; } = JsonSerializerOptions.Default;

    public void WriteCollection<T>(IEnumerable<T> courses, FileInfo output)
    {
        using var stream = output.OpenWrite();
        stream.SetLength(0);
        JsonSerializer.Serialize(stream, courses, Options);
    }

    public void WriteTimeTable(PlanningWave wave, FileInfo output)
    {
        var outputData = new List<TimeTableEntry>(wave.DayCount * wave.SlotsPerDay);

        foreach (var day in ..wave.DayCount)
        {
            foreach (var slot in ..wave.SlotsPerDay)
            {
                if (wave.FinalPlan[slot, day].Count > 0)
                {
                    outputData.Add(new TimeTableEntry((Day)day, slot, wave.FinalPlan[slot, day]));
                }
            }
        }

        var jsonOptions = new JsonSerializerOptions(Options);
        jsonOptions.Converters.Add(new ReducedKursJsonConverter(wave.Courses));

        using var stream = output.OpenWrite();
        JsonSerializer.Serialize(stream, outputData, jsonOptions);
    }

    public IEnumerable<T> ReadCollection<T>(FileInfo input)
    {
        using var stream = input.OpenRead();
        return JsonSerializer.Deserialize<IEnumerable<T>>(stream, Options) ?? throw new JsonException();
    }
}

public record TimeTableEntry(Day Day, int Slot, List<Course> Courses);
