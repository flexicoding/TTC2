using System.Diagnostics;

namespace TTC.Core.Rules;

/// <summary>
/// reduce the amount of courses per slot<br/>
/// multiplies the probability of each slot by <paramref name="modifiers"/>[slot]
/// </summary>
public sealed class ModifyTimeSlots(ImmutableArray<float> modifiers) : Rule
{
    public ImmutableArray<float> Modifiers { get; } = modifiers;

    public override void Apply(PlanningWave wave)
    {
        Debug.Assert(wave.SlotsPerDay == Modifiers.Length);
        wave.EachSlot((hour, day, course, value) => value * Modifiers[hour]);
    }
}
