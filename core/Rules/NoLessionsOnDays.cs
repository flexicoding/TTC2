namespace TTC.Core.Rules;

/// <summary>
/// no courses will be generated on given days<br/>
/// sets all probabilities to 0 for all given days
/// </summary>
public sealed class NoCoursesOnDays(HashSet<Day> days) : Rule
{
    public HashSet<Day> Days { get; } = days;

    public override void Apply(PlanningWave wave)
    {
        wave.EachSlot((_, day, _) => Days.Contains(day), static _ => 0);
    }
}
