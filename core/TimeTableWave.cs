using System.Diagnostics;
using System.Text;

namespace TTC.Core;

public sealed class TimeTableWave
{
    public ImmutableArray<Course> Courses { get; }
    public ImmutableArray<Rule> Rules { get; }
    public ImmutableArray<Person> People { get; }
    public Tensor Wave { get; }
    public TimeTable FinalPlan { get; }
    public ImmutableArray<Day> Days { get; }
    public int SlotsPerDay { get; } = 8;
    public Random Random { get; init; } = Random.Shared;
    private readonly int[] _kurseOrder;

    public ref float this[int hour, Day day, int lession] => ref Wave[hour, (int)day, lession];
    private ref float this[int hour, int day, int lession] => ref Wave[hour, day, lession];

    public TimeTableWave(IEnumerable<Course> courses, IEnumerable<Rule> rules)
        : this([.. courses], [.. rules]) { }

    public TimeTableWave(ImmutableArray<Course> courses, ImmutableArray<Rule> rules)
    {
        Days = [.. Enum.GetValues<Day>().Order()];
        Courses = Guard.ThrowIfNullOrEmpty(courses);
        if (Courses.Select(k => k.Slug).SelectDuplicates().Any())
        {
            throw new ArgumentException("Kurse contains duplicates", nameof(courses));
        }
        People = [.. courses.SelectMany(k => k.People).Distinct()];
        FinalPlan = new(SlotsPerDay, Days.Length);
        Wave = Tensor.Create(SlotsPerDay, Days.Length, courses.Length);
        Wave.Fill(1);
        Rules = rules;
        _kurseOrder = [.. Enumerable.Range(0, Courses.Length)];
    }

    public int Collapse(bool verbose)
    {
        do
        {
            ApplyRules();
        } while (CollapsNext());

        return Validate(verbose);
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
        FinalPlan[hour, (Day)day].Add(course);

        EnforceLessionCount(kursIndex, course.LessionsPerTurnus);

        return true;
    }

    private void EnforceLessionCount(int kursIndex, int lessionsPerTurnus)
    {
        var count = CountCourseLessions(Courses[kursIndex]);

        if (count > lessionsPerTurnus)
        {
            throw new UnreachableException("Somehow a course ended up with two many lessions???");
        }

        if (count == lessionsPerTurnus)
        {
            foreach (var dayIndex in ..Days.Length)
            {
                foreach (var hourIndex in ..SlotsPerDay)
                {
                    this[hourIndex, dayIndex, kursIndex] = 0;
                }
            }
        }
    }

    public int CountCourseLessions(Course course) => FinalPlan.Count(l => l.Contains(course));

    public (int hour, int day, int lession) IndexOfMaximum()
    {
        int mhour = -1, mday = -1, mlession = -1;
        float value = 0;

        // we shuffle to prevent the function from always giving back the first maximum.
        // this appears to be enough randomness to generate a variety of time tables
        Random.Shuffle(_kurseOrder);

        foreach (var lession in _kurseOrder)
        {
            foreach (var day in ..Days.Length)
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
        foreach (var day in Days)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                foreach (var courseIndex in _kurseOrder)
                {
                    if (predicate(hour, day, Courses[courseIndex]))
                    {
                        this[hour, day, courseIndex] = modifier(this[hour, day, courseIndex]);
                    }
                }
            }
        }
    }

    public void EachSlot(Func<int, Day, Course, float, float> modifier)
    {
        foreach (var day in Days)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                foreach (var courseIndex in _kurseOrder)
                {
                    this[hour, day, courseIndex] = modifier(hour, day, Courses[courseIndex], this[hour, day, courseIndex]);
                }
            }
        }
    }

    public void Reset()
    {
        foreach (var day in Days)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                FinalPlan[hour, day].Clear();
            }
        }

        Wave.Fill(1);
    }

    public int Validate(bool verbose)
    {
        var issues = 0;
        // verify each course has the exact amount of lessions
        foreach (var course in Courses)
        {
            var count = CountCourseLessions(course);
            if (count != course.LessionsPerTurnus)
            {
                WriteLine($"{course.Slug} has {count}/{course.LessionsPerTurnus} lessions per turnus");
                issues++;
            }
        }

        // verify max one lession per slot per person
        foreach (var person in People)
        {
            foreach (var day in Days)
            {
                foreach (var slot in ..SlotsPerDay)
                {
                    if (FinalPlan[slot, day].Count(k => k.People.Contains(person)) > 1)
                    {
                        WriteLine($"{person.ID} has more than one lession on {day} {slot}.");
                        issues++;
                    }
                }
            }
        }

        return issues;

        void WriteLine(string message)
        {
            if (verbose)
            {
                Console.WriteLine(message);
            }
        }
    }

    public string ToString(int course)
    {
        var sb = new StringBuilder();
        sb.Append($"   ");
        foreach (var day in Days)
        {
            sb.Append(day.ToString()[..5]);
            sb.Append(' ');
        }
        sb.AppendLine();

        foreach (var hour in ..SlotsPerDay)
        {
            sb.Append($"{hour + 1}. ");
            foreach (var day in Days)
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
}
