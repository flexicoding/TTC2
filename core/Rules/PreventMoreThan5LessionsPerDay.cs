namespace TTC.Core.Rules;

public sealed record PreventMoreThan5LessionsPerDay : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in ..wave.DayCount)
        {
            var lessionCount = Enumerable.Range(0, wave.HoursPerDay).Count(hour => wave.FinalPlan[hour, day] is not null);

            if (lessionCount >= 5)
            {
                foreach (var lession in ..wave.Kurse.Length)
                {
                    foreach (var hour in ..wave.HoursPerDay)
                    {
                        wave[hour, day, lession] = 0;
                    }
                }
            }
        }
    }
}
