namespace TTC.Core;

public sealed record Subject(string Slug, int LessionsPerTurnus, FrozenSet<Person> Teachers, FrozenSet<Person> Students)
{
    public string Slug { get; init; } = Slug.Length <= 4 ? Slug.PadLeft(4) : throw new ArgumentException();

    public IEnumerable<Course> DivideIntoCourses(int averageStudentCount, Random? random = null)
    {
        random ??= Random.Shared;

        var courseCount = (int)float.Round((float)Students.Count / averageStudentCount);

        var students = Students.ToArray();
        random.Shuffle(students);

        foreach (var i in ..(courseCount - 1))
        {
            yield return new Course($"{Slug}.{i + 1}", LessionsPerTurnus, [Teachers.ElementAt(i % Teachers.Count), .. students.AsSpan(averageStudentCount * i, averageStudentCount)]);
        }

        yield return new Course($"{Slug}.{courseCount}", LessionsPerTurnus, [Teachers.ElementAt((courseCount-1) % Teachers.Count), .. students.AsSpan(averageStudentCount * (courseCount - 1))]);
    }
}