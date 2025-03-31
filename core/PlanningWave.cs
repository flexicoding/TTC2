using System.Diagnostics;
using System.Text;

namespace TTC.Core;

public sealed class PlanningWave
{
    public ImmutableArray<Kurs> Kurse { get; }
    public ImmutableArray<Rule> Rules { get; }
    public ImmutableArray<Person> People { get; }
    public Tensor Wave { get; }
    public List<Kurs>[,] FinalPlan { get; }
    public int DayCount { get; }
    public int SlotsPerDay { get; } = 8;
    public Random Random { get; init; } = Random.Shared;
    private readonly int[] _kurseOrder;

    public ref float this[int hour, Day day, int lession] => ref Wave[hour, (int)day, lession];
    public ref float this[int hour, int day, int lession] => ref Wave[hour, day, lession];

    public PlanningWave(IEnumerable<Kurs> lessions, IEnumerable<Rule> rules)
        : this([.. lessions], [.. rules]) { }

    public PlanningWave(ImmutableArray<Kurs> kurse, ImmutableArray<Rule> rules)
    {
        DayCount = Enum.GetValues<Day>().Length;
        Kurse = Guard.ThrowIfNullOrEmpty(kurse);
        People = [.. kurse.SelectMany(k => k.People).Distinct()];
        FinalPlan = new List<Kurs>[SlotsPerDay, DayCount];
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                FinalPlan[hour, day] = [];
            }
        }
        Wave = Tensor.Create(SlotsPerDay, DayCount, kurse.Length);
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
        FinalPlan[hour, day].Add(kurs);

        var count = CountKurs(Kurse[kursIndex]);

        if (count > kurs.LessionsPerTurnus)
        {
            throw new UnreachableException();
        }

        if (count == kurs.LessionsPerTurnus)
        {
            foreach (var dayIndex in ..DayCount)
            {
                foreach (var hourIndex in ..SlotsPerDay)
                {
                    this[hourIndex, dayIndex, kursIndex] = 0;
                }
            }
        }

        return true;
    }

    public int CountKurs(Kurs kurs) => FinalPlan.Cast<List<Kurs>>().Count(l => l.Contains(kurs));

    public (int hour, int day, int lession) IndexOfMaximum()
    {
        int mhour = -1, mday = -1, mlession = -1;
        float value = 0;

        // we shuffle to prevent the function from always giving back the first maximum.
        // this appears to be enough randomness to generate a variety of time tables
        Random.Shuffle(_kurseOrder);

        foreach (var lession in _kurseOrder)
        {
            foreach (var day in ..DayCount)
            {
                foreach (var hour in ..SlotsPerDay)
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

    public void EachSlot(Func<int, Day, Kurs, bool> predicate, Func<float, float> modifier)
    {
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
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

    public void EachSlot(Func<int, Day, Kurs, float, float> modifier)
    {
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                foreach (var kursIndex in _kurseOrder)
                {
                    this[hour, day, kursIndex] = modifier(hour, (Day)day, Kurse[kursIndex], this[hour, day, kursIndex]);
                }
            }
        }
    }

    public string ToString(int kurs)
    {
        var sb = new StringBuilder();
        sb.Append($"   ");
        foreach (var day in ..DayCount)
        {
            sb.Append(((Day)day).ToString()[..5]);
            sb.Append(' ');
        }
        sb.AppendLine();

        foreach (var hour in ..SlotsPerDay)
        {
            sb.Append($"{hour + 1}. ");
            foreach (var day in ..DayCount)
            {
                if (FinalPlan[hour, day].Count > 0)
                {
                    sb.Append(FinalPlan[hour, day][0].Slug);
                }
                else
                {
                    sb.Append(this[hour, day, kurs].ToString("F3"));
                }
                sb.Append(' ');
            }
            sb.AppendLine();

        }

        return sb.ToString();
    }

    public bool Validate()
    {
        foreach (var kurs in Kurse)
        {
            var count = CountKurs(kurs);
            if (count != kurs.LessionsPerTurnus)
            {
                Console.WriteLine($"{kurs.Name} has {count} instead of {kurs.LessionsPerTurnus}");
                return false;
            }
        }

        return true;
    }

    public void Reset()
    {
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                FinalPlan[hour, day].Clear();
            }
        }

        Wave.Fill(1);
    }
}
