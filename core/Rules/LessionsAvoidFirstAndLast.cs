namespace TTC.Core.Rules;

public sealed record LessionsAvoidFirstAndLast(float Modifier) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var lession in ..wave.Lessions.Length)
        {
            foreach (var day in 0..wave.DayCount)
            {
                wave[0, day, lession] *= Modifier;
                wave[5, day, lession] *= Modifier;
            }
        }
    }
}
