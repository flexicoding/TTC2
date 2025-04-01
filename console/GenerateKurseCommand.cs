using System.IO;
using System.Text.Json;
using Ametrin.Serialization;

namespace TTC.Console;

public static class GenerateCoursesCommand
{
    public static void Run(string inputPath, Random seed, JsonSerializerOptions options)
    {
        var input = new FileInfo(inputPath);
        var output = input.Directory!.File($"{input.NameWithoutExtension()}_kurse.json");
        Run(input, output, seed, options);
    }

    public static void Run(string inputPath, string outputPath, Random seed, JsonSerializerOptions options)
    {
        var input = new FileInfo(inputPath);
        var output = Path.IsPathFullyQualified(outputPath) ? new FileInfo(outputPath) : input.Directory!.File(outputPath);
        Run(input, output, seed, options);
    }

    public static void Run(FileInfo input, FileInfo output, Random random, JsonSerializerOptions options)
    {
        var subjects = JsonExtensions.ReadFromJsonFile<ImmutableArray<Subject>>(input, options).OrThrow();
        var courses = subjects.SelectMany(s => s.DivideIntoCourses(20, random)).ToImmutableArray();
        JsonExtensions.WriteToJsonFile(courses, output, options);
    }
}
