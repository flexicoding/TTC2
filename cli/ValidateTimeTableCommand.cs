using TTC.Core.Serialization;

namespace TTC.Cli;

public sealed class ValidateTimeTableCommand
{
    public static void Run(string coursesPath, string timeTablePath, JsonHelper jsonHelper)
    {
        var courses = jsonHelper.ReadCollection<Course>(new(coursesPath));
        var wave = new TimeTableWave(courses, []);
        jsonHelper.FillTimeTable(new(timeTablePath), wave);

        wave.Validate(true);
    }
}
