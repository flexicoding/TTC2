namespace TTC.Core.Rules;

public sealed record AvoidSameKursTwiceADay(float Modifier) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in 0..wave.DayCount)
        {
            foreach (var hour in ..wave.HoursPerDay)
            {
                foreach (var kurs in wave.FinalPlan[hour, day])
                {
                    var kursIndex = wave.Kurse.IndexOf(kurs);
                    foreach (var hi in ..wave.HoursPerDay)
                    {
                        wave[hi, day, kursIndex] *= Modifier;
                    }
                }
            }
        }
    }
}
