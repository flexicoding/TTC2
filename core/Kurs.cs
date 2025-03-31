namespace TTC.Core;

public sealed record Kurs(string Name, string Slug, int LessionsPerTurnus, FrozenSet<Person> People)
{
    public string Slug { get; init; } = Slug.Length <= 5 ? Slug.PadLeft(5) : throw new ArgumentException();
}
