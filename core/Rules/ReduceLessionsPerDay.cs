namespace TTC.Core.Rules;

public sealed record ReduceLessionsPerDay(int MaxLessionsPerDayPerPerson, float ReductionPerLessionPerPerson) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in ..wave.DayCount)
        {
            foreach (var person in wave.People)
            {
                var lessionCount = Enumerable.Range(0, wave.SlotsPerDay).Count(hour => wave.FinalPlan[hour, day].Any(k => k.People.Contains(person)));

                var factor = lessionCount >= MaxLessionsPerDayPerPerson ? 0 : 1 - ReductionPerLessionPerPerson * lessionCount;

                foreach (var lession in ..wave.Kurse.Length)
                {
                    foreach (var hour in ..wave.SlotsPerDay)
                    {
                        wave[hour, day, lession] *= factor;
                    }
                }
            }
        }
    }
}
