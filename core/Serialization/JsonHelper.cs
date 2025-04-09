using System.IO;
using System.Text.Json;

namespace TTC.Core.Serialization;

public sealed class JsonHelper
{
    public JsonSerializerOptions Options { get; set; } = JsonSerializerOptions.Default;

    public void WriteCollection<T>(IEnumerable<T> data, FileInfo output, JsonSerializerOptions? options = null)
    {
        using var stream = output.OpenWrite();
        stream.SetLength(0);
        JsonSerializer.Serialize(stream, data, options ?? Options);
    }

    public void WriteTimeTable(TimeTableWave wave, FileInfo output)
    {
        var outputData = new List<TimeTableEntry>(wave.Days.Length * wave.SlotsPerDay);

        foreach (var day in wave.Days)
        {
            foreach (var slot in ..wave.SlotsPerDay)
            {
                if (wave.FinalPlan[slot, day].Count > 0)
                {
                    outputData.Add(new TimeTableEntry(day, slot, wave.FinalPlan[slot, day]));
                }
            }
        }

        var jsonOptions = new JsonSerializerOptions(Options);
        jsonOptions.Converters.Add(new ReducedCoursesJsonConverter(wave.Courses));

        WriteCollection(outputData, output, jsonOptions);
    }

    public IEnumerable<T> ReadCollection<T>(FileInfo input, JsonSerializerOptions? options = null)
    {
        using var stream = input.OpenRead();
        return JsonSerializer.Deserialize<IEnumerable<T>>(stream, options ?? Options) ?? throw new JsonException();
    }

    public void FillTimeTable(FileInfo input, TimeTableWave wave)
    {
        var jsonOptions = new JsonSerializerOptions(Options);
        jsonOptions.Converters.Add(new ReducedCoursesJsonConverter(wave.Courses));

        var data = ReadCollection<TimeTableEntry>(input, jsonOptions);

        foreach (var entry in data)
        {
            wave.FinalPlan[entry.Slot, entry.Day].AddRange(entry.Courses);
        }
    }
}

public record TimeTableEntry(Day Day, int Slot, List<Course> Courses);
