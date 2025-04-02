namespace TTC.Core.Rules;

public sealed class ReduceLessionsPerDay(int MaxLessionsPerDayPerPerson, float ReductionPerLessionPerPerson) : Rule
{
    public int MaxLessionsPerDayPerPerson { get; } = MaxLessionsPerDayPerPerson;
    public float ReductionPerLessionPerPerson { get; } = ReductionPerLessionPerPerson;

    public override void Apply(TimeTableWave wave)
    {
        foreach (var day in wave.Days)
        {
            foreach (var person in wave.People)
            {
                var lessionCount = Enumerable.Range(0, wave.SlotsPerDay).Count(hour => wave.FinalPlan[hour, day].Any(k => k.People.Contains(person)));

                var factor = lessionCount >= MaxLessionsPerDayPerPerson ? 0 : 1 - ReductionPerLessionPerPerson * lessionCount;

                foreach (var course in ..wave.Courses.Length)
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
