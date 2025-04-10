namespace TTC.Core.Rules;

public sealed class MatchingSlotRule : Rule
{
    public FrozenSet<Day> Days { get; init; } = [];
    public FrozenSet<int> Slots { get; init; } = [];
    public FrozenSet<string> Courses { get; init; } = [];
    public required float Modifier { get; init; }

    public override void Apply(TimeTableWave wave)
    {
        var courses = Courses.Count is 0 ? wave.Courses : wave.Courses.Where(c => Courses.Contains(c.Slug));
        var days = Days.Count is 0 ? wave.Days : Days;
        var slots = Slots.Count is 0 ? Enumerable.Range(0, wave.SlotsPerDay) : Slots;
        foreach (var course in courses)
        {
            foreach (var day in days)
            {
                foreach (var slot in slots)
                {
                    wave[slot, day, course] *= Modifier;
                }
            }
        }
    }

    private bool Matches(Course course, Day day, int slot)
        => Days.Contains(day) && Slots.Contains(slot) && Courses.Contains(course.Slug);
}
