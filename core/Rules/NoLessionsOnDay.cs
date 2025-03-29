namespace TTC.Core.Rules;

public sealed record NoLessionsOnDay(Day Day) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        wave.ApplyRule((_, day, _) => day == Day, static _ => 0);
    }
}
