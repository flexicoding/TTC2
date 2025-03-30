namespace TTC.Core.Rules;

public sealed record AvoidFirstAndLastTimeSlot(float Modifier) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var kurs in ..wave.Kurse.Length)
        {
            foreach (var day in 0..wave.DayCount)
            {
                wave[0, day, kurs] *= Modifier;
                wave[5, day, kurs] *= Modifier;
            }
        }
    }
}
