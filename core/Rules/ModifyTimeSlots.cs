using System.Diagnostics;

namespace TTC.Core.Rules;

public sealed record ModifyTimeSlots(ImmutableArray<float> Modifiers) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        Debug.Assert(wave.SlotsPerDay == Modifiers.Length );
        wave.EachSlot((hour, day, kurs, value) => value * Modifiers[hour]);
    }
}
