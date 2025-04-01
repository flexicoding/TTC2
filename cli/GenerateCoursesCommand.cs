using System.IO;
using Ametrin.Serialization;
using TTC.Core.Serialization;

namespace TTC.Cli;

public static class GenerateCoursesCommand
{
    public static void Run(string inputPath, Random seed, JsonHelper helper)
    {
        var input = new FileInfo(inputPath);
        var output = input.Directory!.File($"{input.NameWithoutExtension()}_kurse.json");
        Run(input, output, seed, helper);
    }

    public static void Run(string inputPath, string outputPath, Random seed, JsonHelper helper)
    {
        var input = new FileInfo(inputPath);
        var output = Path.IsPathFullyQualified(outputPath) ? new FileInfo(outputPath) : input.Directory!.File(outputPath);
        Run(input, output, seed, helper);
    }

    public static void Run(FileInfo input, FileInfo output, Random random, JsonHelper helper)
    {
        var subjects = JsonExtensions.ReadFromJsonFile<ImmutableArray<Subject>>(input, helper.Options).OrThrow();
        var courses = subjects.SelectMany(s => s.DivideIntoCourses(20, random)).ToImmutableArray();
        JsonExtensions.WriteToJsonFile(courses, output, helper.Options);
    }
}
