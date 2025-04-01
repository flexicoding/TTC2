using System.Diagnostics;
using System.Text;

namespace TTC.Core;

public sealed class PlanningWave
{
    public ImmutableArray<Course> Courses { get; }
    public ImmutableArray<Rule> Rules { get; }
    public ImmutableArray<Person> People { get; }
    public Tensor Wave { get; }
    public List<Course>[,] FinalPlan { get; }
    public int DayCount { get; }
    public int SlotsPerDay { get; } = 8;
    public Random Random { get; init; } = Random.Shared;
    private readonly int[] _kurseOrder;

    public ref float this[int hour, Day day, int lession] => ref Wave[hour, (int)day, lession];
    public ref float this[int hour, int day, int lession] => ref Wave[hour, day, lession];

    public PlanningWave(IEnumerable<Course> courses, IEnumerable<Rule> rules)
        : this([.. courses], [.. rules]) { }

    public PlanningWave(ImmutableArray<Course> courses, ImmutableArray<Rule> rules)
    {
        DayCount = Enum.GetValues<Day>().Length;
        Courses = Guard.ThrowIfNullOrEmpty(courses);
        if (Courses.Select(k => k.Slug).SelectDuplicates().Any())
        {
            throw new ArgumentException("Kurse contains duplicates", nameof(courses));
        }
        People = [.. courses.SelectMany(k => k.People).Distinct()];
        FinalPlan = new List<Course>[SlotsPerDay, DayCount];
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                FinalPlan[hour, day] = [];
            }
        }
        Wave = Tensor.Create(SlotsPerDay, DayCount, courses.Length);
        Wave.Fill(1);
        Rules = rules;
        _kurseOrder = [.. Enumerable.Range(0, Courses.Length)];
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

        var course = Courses[kursIndex];
        FinalPlan[hour, day].Add(course);

        var count = CountCourse(Courses[kursIndex]);

        if (count > course.LessionsPerTurnus)
        {
            throw new UnreachableException();
        }

        if (count == course.LessionsPerTurnus)
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

    public int CountCourse(Course course) => FinalPlan.Cast<List<Course>>().Count(l => l.Contains(course));

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

    public void EachSlot(Func<int, Day, Course, bool> predicate, Func<float, float> modifier)
    {
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                foreach (var courseIndex in _kurseOrder)
                {
                    if (predicate(hour, (Day)day, Courses[courseIndex]))
                    {
                        this[hour, day, courseIndex] = modifier(this[hour, day, courseIndex]);
                    }
                }
            }
        }
    }

    public void EachSlot(Func<int, Day, Course, float, float> modifier)
    {
        foreach (var day in ..DayCount)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                foreach (var courseIndex in _kurseOrder)
                {
                    this[hour, day, courseIndex] = modifier(hour, (Day)day, Courses[courseIndex], this[hour, day, courseIndex]);
                }
            }
        }
    }

    public string ToString(int course)
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
                    sb.Append(this[hour, day, course].ToString("F3"));
                }
                sb.Append(' ');
            }
            sb.AppendLine();

        }

        return sb.ToString();
    }

    public bool Validate()
    {
        foreach (var course in Courses)
        {
            var count = CountCourse(course);
            if (count != course.LessionsPerTurnus)
            {
                Console.WriteLine($"{course.Slug} has {count} lessions per turnus instead of {course.LessionsPerTurnus}");
                return false;
            }
        }

        foreach (var person in People)
        {
            foreach (var day in ..DayCount)
            {
                foreach (var slot in ..SlotsPerDay)
                {
                    if (FinalPlan[slot, day].Count(k => k.People.Contains(person)) > 1)
                    {
                        Console.WriteLine($"{person.ID} has more than one lession on {(Day)day} {slot}.");
                        return false;
                    }
                }
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
