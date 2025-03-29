namespace TTC.Core;

public abstract record Rule
{
    public abstract void Apply(PlanningWave wave);
}
