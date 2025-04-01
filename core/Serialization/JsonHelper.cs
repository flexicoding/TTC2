using System.IO;
using System.Text.Json;
using Ametrin.Serialization;

namespace TTC.Core.Serialization;

public sealed class JsonHelper
{
    public JsonSerializerOptions Options { get; set; } = JsonSerializerOptions.Default;

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

        JsonExtensions.WriteToJsonFile(outputData, output, jsonOptions);
    }
}

public record TimeTableEntry(Day Day, int Slot, List<Course> Courses);
