namespace TTC.Core.Rules;

public sealed record NoLessionsOnDays(HashSet<Day> Days) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        wave.EachSlot((_, day, _) => Days.Contains(day), static _ => 0);
    }
}
