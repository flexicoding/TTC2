namespace TTC.Core.Rules;

public sealed class RequireOneLessionPerPersonPerSlot : Rule
{
    public override void Apply(TimeTableWave wave)
    {
        foreach (var day in wave.Days)
        {
            foreach (var hour in ..wave.SlotsPerDay)
            {
                var occupiedPeople = wave.FinalPlan[hour, day].SelectMany(static c => c.People).ToFrozenSet();

                if (occupiedPeople.Count is 0) continue;

                foreach (var course in wave.Courses)
                {
                    if (course.People.Overlaps(occupiedPeople))
                    {
                        wave[hour, day, course] = 0;
                    }
                }
            }
        }
    }
}