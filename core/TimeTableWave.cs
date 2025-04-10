using System.Diagnostics;
using System.Text;

namespace TTC.Core;

public sealed class TimeTableWave
{
    public ImmutableArray<Course> Courses { get; }
    private FrozenDictionary<Course, int> CoursesLookup { get; }
    public ImmutableArray<Rule> Rules { get; }
    public ImmutableArray<Person> People { get; }
    public Tensor Wave { get; }
    public TimeTable FinalPlan { get; }
    public FrozenSet<Day> Days { get; }
    public int SlotsPerDay { get; } = 8;
    public Random Random { get; init; } = Random.Shared;
    private readonly int[] _coursesOrder;

    public ref float this[int hour, Day day, int courseIndex] => ref Wave[hour, (int)day, courseIndex];
    public ref float this[int hour, Day day, Course course] => ref Wave[hour, (int)day, CoursesLookup[course]];
    private ref float this[int hour, int day, int courseIndex] => ref Wave[hour, day, courseIndex];

    public TimeTableWave(IEnumerable<Course> courses, IEnumerable<Rule> rules)
        : this([.. courses], [.. rules]) { }

    public TimeTableWave(ImmutableArray<Course> courses, ImmutableArray<Rule> rules)
    {
        Days = [.. Enum.GetValues<Day>().Order()];
        Courses = Guard.ThrowIfNullOrEmpty(courses);
        if (Courses.Select(k => k.Slug).SelectDuplicates().Any())
        {
            throw new ArgumentException("Courses contains duplicates", nameof(courses));
        }
        CoursesLookup = Courses.Select((c, i) => new KeyValuePair<Course, int>(c, i)).ToFrozenDictionary();
        People = [.. courses.SelectMany(k => k.People).Distinct()];
        FinalPlan = new(SlotsPerDay, Days.Count);
        Wave = Tensor.Create(SlotsPerDay, Days.Count, courses.Length);
        Wave.Fill(1);
        Rules = rules;
        _coursesOrder = [.. Enumerable.Range(0, Courses.Length)];
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

        return true;
    }

    public int CountCourseLessons(Course course) => FinalPlan.Count(l => l.Contains(course));

    public (int hour, int day, int courseIndex) IndexOfMaximum()
    {
        int mhour = -1, mday = -1, mcourseIndex = -1;
        float value = 0;

        // we shuffle to prevent the function from always giving back the first maximum.
        // this appears to be enough randomness to generate a variety of time tables
        Random.Shuffle(_coursesOrder);

        foreach (var course in _coursesOrder)
        {
            foreach (var day in ..Days.Count)
            {
                foreach (var hour in ..SlotsPerDay)
                {
                    if (this[hour, day, course] > value)
                    {
                        (mhour, mday, mcourseIndex) = (hour, day, course);
                        value = this[hour, day, course];
                    }
                }
            }
        }

        return (mhour, mday, mcourseIndex);
    }

    public void EachSlot(Func<int, Day, Course, bool> predicate, Func<float, float> modifier)
    {
        foreach (var day in Days)
        {
            foreach (var hour in ..SlotsPerDay)
            {
                foreach (var courseIndex in _coursesOrder)
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
                foreach (var courseIndex in _coursesOrder)
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
            foreach (var slot in ..SlotsPerDay)
            {
                FinalPlan[slot, day].Clear();
            }
        }

        Wave.Fill(1);
    }

    public int Validate(bool verbose)
    {
        var issues = 0;
        // verify each course has the exact amount of lessons
        foreach (var course in Courses)
        {
            var count = CountCourseLessons(course);
            if (count != course.LessonsPerTurnus)
            {
                WriteLine($"{course.Slug} has {count}/{course.LessonsPerTurnus} lessons per turnus");
                issues++;
            }
        }

        // verify max one lesson per slot per person
        foreach (var person in People)
        {
            foreach (var day in Days)
            {
                foreach (var slot in ..SlotsPerDay)
                {
                    if (FinalPlan[slot, day].Count(k => k.People.Contains(person)) > 1)
                    {
                        WriteLine($"{person.ID} has more than one lesson on {day} {slot}.");
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
                    sb.Append(FinalPlan[hour, day][0].Slug.PadLeft(5));
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

    public override string ToString()
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
                    sb.Append(FinalPlan[hour, day][0].Slug.PadLeft(5));
                }
                else
                {
                    sb.Append("   --");
                }
                sb.Append(' ');
            }
            sb.AppendLine();

        }

        return sb.ToString();
    }
}
