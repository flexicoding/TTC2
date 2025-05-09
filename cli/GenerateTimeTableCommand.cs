using System.IO;
using System.Text.Json;
using Ametrin.Serialization;
using TTC.Core.Serialization;

namespace TTC.Cli;

public static class GenerateTimeTableCommand
{
    public static void Run(string inputPath, Random random, string? outputPath, string rulesPath, string? existingPath, bool verbose, JsonHelper jsonHelper)
    {
        var input = new FileInfo(inputPath);
        var output = input.Directory!.File(outputPath ?? $"{input.NameWithoutExtension()}_timetable.json");
        using var stream = input.OpenRead();
        var kurse = JsonSerializer.Deserialize<IEnumerable<Course>>(stream, jsonHelper.Options) ?? throw new NullReferenceException();

        var rules = Directory.EnumerateFiles(rulesPath, "*.json").Select(f => JsonExtensions.ReadFromJsonFile<Rule>(f, jsonHelper.Options).OrThrow());

        var wave = new TimeTableWave(kurse, rules)
        {
            Random = random
        };

        if (wave.Rules.Length is 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Rules found! Canceling...");
            Console.ResetColor();
            return;
        }

        if (existingPath is not null)
        {
            jsonHelper.FillTimeTable(new(existingPath), wave);
        }

        var count = 1;

        while (wave.Collapse(verbose) > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (!verbose)
            {
                Console.CursorLeft = 0;
                Console.CursorTop--;
            }
            Console.WriteLine($"Failed to generate time table! Trying again... ({count})");
            Console.ResetColor();
            wave.Reset();
            count++;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Successfully generated table ({count} Attempts)");
        Console.ResetColor();

        jsonHelper.WriteTimeTable(wave, output);
    }
}
