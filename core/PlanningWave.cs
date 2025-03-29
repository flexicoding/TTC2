using System.Text;

namespace TTC.Core;

public sealed class PlanningWave
{
    public ImmutableArray<Lession> Lessions { get; }
    public ImmutableArray<Rule> Rules { get; }
    public Tensor Wave { get; }
    public Lession?[,] FinalPlan { get; }
    public int DayCount { get; }
    public int LessionsPerDay { get; } = 6;
    private readonly int[] _lessionsOrder;

    public ref float this[int hour, Day day, int lession] => ref Wave[hour, (int)day, lession];
    public ref float this[int hour, int day, int lession] => ref Wave[hour, day, lession];

    public PlanningWave(IEnumerable<Lession> lessions, IEnumerable<Rule> rules)
        : this([.. lessions], [.. rules]) { }

    public PlanningWave(ImmutableArray<Lession> kurse, ImmutableArray<Rule> rules)
    {
        DayCount = Enum.GetValues<Day>().Length;
        Lessions = Guard.ThrowIfNullOrEmpty(kurse);
        FinalPlan = new Lession?[LessionsPerDay, DayCount];
        Wave = Tensor.Create(LessionsPerDay, DayCount, kurse.Length);
        Wave.Fill(1);
        Rules = rules;
        _lessionsOrder = Enumerable.Range(0, Lessions.Length).ToArray();
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
        var (hour, day, lession) = IndexOfMaximum();

        if (lession < 0) return false;

        FinalPlan[hour, day] = Lessions[lession];
        foreach (var i in ..Lessions.Length)
        {
            this[hour, day, i] = 0;
        }

        foreach (var dayIndex in ..DayCount)
        {
            foreach (var hourIndex in ..LessionsPerDay)
            {
                this[hourIndex, dayIndex, lession] = 0;
            }
        }

        return true;
    }

    public (int hour, int day, int lession) IndexOfMaximum()
    {
        int mhour = -1, mday = -1, mlession = -1;
        float value = 0;

        Random.Shared.Shuffle(_lessionsOrder);

        foreach (var lession in _lessionsOrder)
        {
            foreach (var day in ..DayCount)
            {
                foreach (var hour in ..LessionsPerDay)
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

        foreach (var hour in ..LessionsPerDay)
        {
            sb.Append($"{hour + 1}. ");
            foreach (var day in ..DayCount)
            {
                if (FinalPlan[hour, day] is Lession collapsed)
                {
                    sb.Append(collapsed.Kurs.Slug);
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
