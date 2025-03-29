namespace TTC.Core.Rules;

public sealed record PreventMoreThan5LessionsPerDay : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in ..wave.DayCount)
        {
            var lessionCount = 0;
            foreach (var hour in ..wave.LessionsPerDay)
            {
                if (wave.FinalPlan[hour, day] is not null)
                {
                    lessionCount++;
                }
            }
            if (lessionCount >= 5)
            {
                foreach (var lession in ..wave.Lessions.Length)
                {
                    foreach (var hour in ..wave.LessionsPerDay)
                    {
                        wave[hour, day, lession] = 0;
                    }
                }
            }
        }
    }
}
