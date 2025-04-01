using System.IO;
using Ametrin.Serialization;
using TTC.Core.Serialization;

namespace TTC.Cli;

public static class GenerateCoursesCommand
{
    public static void Run(string inputPath, Random random, string? outputPath, JsonHelper jsonHelper)
    {
        var input = new FileInfo(inputPath);
        var output = outputPath is null
            ? input.Directory!.File($"{input.NameWithoutExtension()}_kurse.json")
            : Path.IsPathFullyQualified(outputPath)
                ? new FileInfo(outputPath)
                : input.Directory!.File(outputPath);

        var subjects = JsonExtensions.ReadFromJsonFile<ImmutableArray<Subject>>(input, jsonHelper.Options).OrThrow();
        var courses = subjects.SelectMany(s => s.DivideIntoCourses(20, random)).ToImmutableArray();
        jsonHelper.WriteCollection(courses, output);
    }
}
