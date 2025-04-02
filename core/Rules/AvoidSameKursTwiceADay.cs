namespace TTC.Core.Rules;

/// <summary>
/// makes it unlikely for a course to get more than one slot per day<br/>
/// multiplies the probability of the same course placed on that day again by <paramref name="modifier"/>
/// </summary>
public sealed class AvoidSameCourseTwiceADay(float modifier) : Rule
{
    public float Modifier { get; } = modifier;

    public override void Apply(TimeTableWave wave)
    {
        foreach (var day in wave.Days)
        {
            foreach (var hour in ..wave.SlotsPerDay)
            {
                foreach (var course in wave.FinalPlan[hour, day])
                {
                    var kursIndex = wave.Courses.IndexOf(course);
                    foreach (var hi in ..wave.SlotsPerDay)
                    {
                        wave[hi, day, kursIndex] *= Modifier;
                    }
                }
            }
        }
    }
}
