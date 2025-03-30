namespace TTC.Core.Rules;

public sealed record ReduceLessionsPerDay(int MaxLessionsPerDay, float ReductionPerLession) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in ..wave.DayCount)
        {
            var lessionCount = Enumerable.Range(0, wave.HoursPerDay).Count(hour => wave.FinalPlan[hour, day] is not null);

            var factor = lessionCount >= MaxLessionsPerDay ? 0 : 1 - ReductionPerLession * lessionCount;

            foreach (var lession in ..wave.Kurse.Length)
            {
                foreach (var hour in ..wave.HoursPerDay)
                {
                    wave[hour, day, lession] *= factor;
                }
            }
        }
    }
}
