namespace TTC.Core;

public sealed record Subject(string Slug, int LessionsPerTurnus, FrozenSet<Person> Teachers, FrozenSet<Person> Students)
{
    public IEnumerable<Course> DivideIntoCourses(int averageStudentCount, Random? random = null)
    {
        random ??= Random.Shared;

        var courseCount = (int)float.Round((float)Students.Count / averageStudentCount);
        courseCount = int.Max(courseCount, 1);

        var students = Students.ToArray();
        random.Shuffle(students);

        foreach (var i in ..(courseCount - 1))
        {
            yield return new Course($"{Slug}{i + 1}", LessionsPerTurnus, [Teachers.ElementAt(i % Teachers.Count), .. students.AsSpan(averageStudentCount * i, averageStudentCount)]);
        }

        yield return new Course($"{Slug}{courseCount}", LessionsPerTurnus, [Teachers.ElementAt((courseCount-1) % Teachers.Count), .. students.AsSpan(averageStudentCount * (courseCount - 1))]);
    }
}