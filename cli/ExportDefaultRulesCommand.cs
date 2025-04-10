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
            new ModifyTimeSlots([0.5f, 1, 1.1f, 1.1f, 1, 0.6f, 0.005f, 0f]) { Name = "weight_time_slots" },
            new AvoidSameCourseTwiceADay(0.1f) { Name = "prefer_course_once_per_day" },
            new ReduceLessonsPerDay(5, 0.98f) { Name = "reduce_lessons_per_day" },
            new MatchingSlotRule { Days = [Day.ASonntag, Day.BFreitag, Day.BSamstag, Day.BSonntag], Modifier = 0, Name = "no_lessons_on_days" },
            new RequireOneLessonPerPersonPerSlot() { Name = "single_lesson_per_person_per_slot" },
            new EnforceCourseLessonCount() { Name = "enforce_course_lesson_count" },
        ];

        foreach (var rule in rules)
        {
            using var stream = output.File($"{rule.Name}.json").OpenWrite();
            JsonSerializer.Serialize(stream, rule, jsonHelper.Options);
        }

        Console.WriteLine($"Exported rules to {output.FullName}");
    }
}