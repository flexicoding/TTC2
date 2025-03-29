using System.Diagnostics;
using System.Text;

namespace TTC.Core;

public sealed class PlanningWave
{
    public ImmutableArray<Kurs> Kurse { get; }
    public ImmutableArray<Rule> Rules { get; }
    public Tensor Wave { get; }
    public Kurs?[,] FinalPlan { get; }
    public int DayCount { get; }
    public int HoursPerDay { get; } = 6;
    private readonly int[] _kurseOrder;

    public ref float this[int hour, Day day, int lession] => ref Wave[hour, (int)day, lession];
    public ref float this[int hour, int day, int lession] => ref Wave[hour, day, lession];

    public PlanningWave(IEnumerable<Kurs> lessions, IEnumerable<Rule> rules)
        : this([.. lessions], [.. rules]) { }

    public PlanningWave(ImmutableArray<Kurs> kurse, ImmutableArray<Rule> rules)
    {
        DayCount = Enum.GetValues<Day>().Length;
        Kurse = Guard.ThrowIfNullOrEmpty(kurse);
        FinalPlan = new Kurs?[HoursPerDay, DayCount];
        Wave = Tensor.Create(HoursPerDay, DayCount, kurse.Length);
        Wave.Fill(1);
        Rules = rules;
        _kurseOrder = [.. Enumerable.Range(0, Kurse.Length)];
    }

    public void ApplyRules()
    {
        foreach (var i in ..Wave.FlatCount)
        {
            if (Wave[i] > 0)
            {
                Wave[i] = 1;
            }
        }

        foreach (var rule in Rules)
        {
            rule.Apply(this);
        }
    }

    public bool CollapsNext()
    {
        var (hour, day, kursIndex) = IndexOfMaximum();

        if (kursIndex < 0) return false;

        var kurs = Kurse[kursIndex];
        FinalPlan[hour, day] = kurs;
        foreach (var i in ..Kurse.Length)
        {
            this[hour, day, i] = 0;
        }

        var count = CountKurs(Kurse[kursIndex]);

        if (count > kurs.LessionCount)
        {
            throw new UnreachableException();
        }

        if (count == kurs.LessionCount)
        {
            foreach (var dayIndex in ..DayCount)
            {
                foreach (var hourIndex in ..HoursPerDay)
                {
                    this[hourIndex, dayIndex, kursIndex] = 0;
                }
            }
        }

        return true;
    }

    public int CountKurs(Kurs kurs) => FinalPlan.Cast<Kurs?>().Where(l => l == kurs).Count();

    public (int hour, int day, int lession) IndexOfMaximum()
    {
        int mhour = -1, mday = -1, mlession = -1;
        float value = 0;

        // we shuffle to prevent the function from always giving back the first maximum.
        // this appears to be enough randomness to generate a variety of time tables
        Random.Shared.Shuffle(_kurseOrder);

        foreach (var lession in _kurseOrder)
        {
            foreach (var day in ..DayCount)
            {
                foreach (var hour in ..HoursPerDay)
                {
                    if (this[hour, day, lession] > value)
                    {
                        (mhour, mday, mlession) = (hour, day, lession);
                        value = this[hour, day, lession];
                    }
                }
            }
        }

        return (mhour, mday, mlession);
    }

    public void ApplyRule(Func<int, Day, Kurs, bool> predicate, Func<float, float> modifier)
    {
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..HoursPerDay)
            {
                foreach (var kursIndex in _kurseOrder)
                {
                    if (predicate(hour, (Day)day, Kurse[kursIndex]))
                    {
                        this[hour, day, kursIndex] = modifier(this[hour, day, kursIndex]);
                    }
                }
            }
        }
    }

    public string ToString(int lession)
    {
        var sb = new StringBuilder();
        sb.Append($"   ");
        foreach (var day in ..DayCount)
        {
            sb.Append(((Day)day).ToString()[..4]);
            sb.Append(' ');
        }
        sb.AppendLine();

        foreach (var hour in ..HoursPerDay)
        {
            sb.Append($"{hour + 1}. ");
            foreach (var day in ..DayCount)
            {
                if (FinalPlan[hour, day] is Kurs collapsed)
                {
                    sb.Append(collapsed.Slug);
                }
                else
                {
                    sb.Append(this[hour, day, lession].ToString("F2"));
                }
                sb.Append(' ');
            }
            sb.AppendLine();

        }

        return sb.ToString();
    }
}
