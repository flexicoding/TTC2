namespace TTC.Core;

public sealed record Course(string Slug, int LessionsPerTurnus, FrozenSet<Person> People)
{
    public string Slug { get; init; } = Slug.Length <= 5 ? Slug : throw new ArgumentException();
}
