namespace TTC.Core.Rules;

public sealed class RequireOneLessionPerPersonPerSlot : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in ..wave.DayCount)
        {
            foreach (var hour in ..wave.SlotsPerDay)
            {
                foreach (var course in wave.FinalPlan[hour, day])
                {
                    foreach (var person in course.People)
                    {
                        foreach (var otherCourse in ..wave.Courses.Length)
                        {
                            if (wave.Courses[otherCourse].People.Contains(person))
                            {
                                wave[hour, day, otherCourse] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}