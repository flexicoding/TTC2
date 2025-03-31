namespace TTC.Core;

public sealed record Subject(string Name, string Slug, int LessionsPerTurnus, FrozenSet<Person> Teachers, FrozenSet<Person> Students)
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
            yield return new Kurs(Name, $"{Slug}.{i + 1}", LessionsPerTurnus, [Teachers.GetRandomElement(random), .. students.AsSpan(averageStudentCount * i, averageStudentCount)]);
        }

        yield return new Kurs(Name, $"{Slug}.{kursCount}", LessionsPerTurnus, [Teachers.GetRandomElement(random), .. students.AsSpan(averageStudentCount * (kursCount - 1))]);

    }
}