namespace TTC.Core.Rules;

public sealed class ReduceLessonsPerDay(int MaxLessonsPerDayPerPerson, float ReductionPerLessonPerPerson) : Rule
{
    public int MaxLessonsPerDayPerPerson { get; } = MaxLessonsPerDayPerPerson;
    public float ReductionPerLessonPerPerson { get; } = ReductionPerLessonPerPerson;

    public override void Apply(TimeTableWave wave)
    {
        foreach (var day in wave.Days)
        {
            foreach (var person in wave.People)
            {
                var lessonCount = Enumerable.Range(0, wave.SlotsPerDay).Count(hour => wave.FinalPlan[hour, day].Any(k => k.People.Contains(person)));

                var factor = lessonCount >= MaxLessonsPerDayPerPerson ? 0 : float.Pow(ReductionPerLessonPerPerson, lessonCount) ;

                foreach (var course in wave.Courses.Where(c => c.People.Contains(person)))
                {
                    foreach (var hour in ..wave.SlotsPerDay)
                    {
                        wave[hour, day, course] *= factor;
                    }
                }
            }
        }
    }
}
