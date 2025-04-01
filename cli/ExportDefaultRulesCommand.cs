using System.IO;
using System.Text.Json;
using TTC.Core.Rules;
using TTC.Core.Serialization;

namespace TTC.Cli;

public static class ExportDefaultRulesCommand
{
    public static void Run(string? outputPath, JsonHelper jsonHelper)
    {
        var output = new DirectoryInfo(outputPath ?? "Rules");
        output.CreateIfNotExists();

        IEnumerable<Rule> rules = [
            new ModifyTimeSlots([0.6f, 1, 1, 1, 1, 0.6f, 0.1f, 0.05f]) { Name = "weight_time_slots" },
            new AvoidSameCourseTwiceADay(0.1f) { Name = "avoid_kurs_more_than_once_per_day" },
            new ReduceLessionsPerDay(5, 0.01f) { Name = "reduce_lessions_per_day" },
            new NoCoursesOnDays([Day.ASonntag, Day.BFreitag, Day.BSamstag, Day.BSonntag]) { Name = "no_lessions_on_days" },
            new RequireOneLessionPerPersonPerSlot() { Name = "require_one_lession_per_person_per_slot" },
        ];

        foreach (var rule in rules)
        {
            using var stream = output.File($"{rule.Name}.json").OpenWrite();
            JsonSerializer.Serialize(stream, rule, jsonHelper.Options);
        }
    }
}