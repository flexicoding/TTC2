namespace TTC.Core.Rules;

public sealed class EnforceCourseLessonCount : Rule
{
    public override void Apply(TimeTableWave wave)
    {
        foreach (var course in wave.Courses)
        {
            var count = wave.CountCourseLessons(course);

            if (count > course.LessonsPerTurnus)
            {
                throw new UnreachableException("Somehow a course ended up with two many lessons???");
            }

            if (count == course.LessonsPerTurnus)
            {
                foreach (var day in wave.Days)
                {
                    foreach (var slot in ..wave.SlotsPerDay)
                    {
                        wave[slot, day, course] = 0;
                    }
                }
            }
        }
    }
}
