namespace TTC.Core;

public sealed record Kurs(string Slug, int LessionsPerTurnus, FrozenSet<Person> People)
{
    public string Slug { get; init; } = Slug.Length <= 5 ? Slug : throw new ArgumentException();
}
