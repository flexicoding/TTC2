namespace TTC.Core;

public sealed record Subject(string Slug, int LessionsPerTurnus, FrozenSet<Person> Teachers, FrozenSet<Person> Students)
{
    public string Slug { get; init; } = Slug.Length <= 4 ? Slug.PadLeft(4) : throw new ArgumentException();

    public IEnumerable<Kurs> DivideIntoKurse(int averageStudentCount, Random? random = null)
    {
        random ??= Random.Shared;

        var kursCount = (int)float.Round((float)Students.Count / averageStudentCount);

        var students = Students.ToArray();
        random.Shuffle(students);

        foreach (var i in ..(kursCount - 1))
        {
            yield return new Kurs($"{Slug}.{i + 1}", LessionsPerTurnus, [Teachers.ElementAt(i % Teachers.Count), .. students.AsSpan(averageStudentCount * i, averageStudentCount)]);
        }

        yield return new Kurs($"{Slug}.{kursCount}", LessionsPerTurnus, [Teachers.ElementAt((kursCount-1) % Teachers.Count), .. students.AsSpan(averageStudentCount * (kursCount - 1))]);
    }
}