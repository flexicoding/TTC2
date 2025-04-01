using System.IO;
using System.Text.Json;
using Ametrin.Serialization;
using TTC.Core.Serialization;

namespace TTC.Cli;

public static class GenerateTimeTableCommand
{
    public static void Run(string inputPath, Random random, string? outputPath, string rulesPath, JsonHelper jsonHelper)
    {
        var input = new FileInfo(inputPath);
        var output = input.Directory!.File(outputPath ?? $"{input.NameWithoutExtension()}_timetable.json");
        using var stream = input.OpenRead();
        var kurse = JsonSerializer.Deserialize<IEnumerable<Course>>(stream, jsonHelper.Options) ?? throw new NullReferenceException();

        var rules = Directory.EnumerateFiles(rulesPath, "*.json").Select(f => JsonExtensions.ReadFromJsonFile<Rule>(f, jsonHelper.Options).OrThrow());

        var wave = new PlanningWave(kurse, rules)
        {
            Random = random
        };

        do
        {
            wave.ApplyRules();
        }
        while (wave.CollapsNext());

        if (!wave.Validate())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to generate time table! Try again");
            Console.ResetColor();

            return;
        }
        jsonHelper.WriteTimeTable(wave, output);
    }
}