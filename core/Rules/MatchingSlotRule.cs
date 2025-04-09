namespace TTC.Core.Rules;

public sealed class MatchingSlotRule : Rule
{
    public FrozenSet<Day> Days { get; init; } = [];
    public FrozenSet<int> Slots { get; init; } = [];
    public FrozenSet<string> Courses { get; init; } = [];
    public required float Modifier { get; init; }

    public override void Apply(TimeTableWave wave)
    {
        foreach (var course in wave.Courses)
        {
            foreach (var day in wave.Days)
            {
                foreach (var slot in ..wave.SlotsPerDay)
                {
                    if (Matches(course, day, slot))
                    {
                        wave[slot, day, course] *= Modifier;
                    }
                }
            }
        }
    }

    private bool Matches(Course course, Day day, int slot)
        => Days.Contains(day) && Slots.Contains(slot) && Courses.Contains(course.Slug);
}
