namespace TTC.Core.Rules;

public sealed record AvoidSameKursOnADay(float Modifier) : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in 0..wave.DayCount)
        {
            foreach (var hour in ..wave.LessionsPerDay)
            {
                if (wave.FinalPlan[hour, day] is Lession lession)
                {
                    foreach (var li in ..wave.Lessions.Length)
                    {
                        if (wave.Lessions[li].Kurs == lession.Kurs)
                        {
                            foreach (var hi in ..wave.LessionsPerDay)
                            {
                                wave[hi, day, li] *= Modifier;
                            }
                        }
                    }
                }
            }
        }
    }
}
