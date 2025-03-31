namespace TTC.Core.Rules;

public sealed record RequireOneLessionPerPersonPerSlot : Rule
{
    public override void Apply(PlanningWave wave)
    {
        foreach (var day in ..wave.DayCount)
        {
            foreach (var hour in ..wave.SlotsPerDay)
            {
                foreach (var kurs in wave.FinalPlan[hour, day])
                {
                    foreach (var person in kurs.People)
                    {
                        foreach (var otherKurs in ..wave.Kurse.Length)
                        {
                            if (wave.Kurse[otherKurs].People.Contains(person))
                            {
                                wave[hour, day, otherKurs] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}