namespace TTC.Core;

public sealed record Course(string Slug, int LessionsPerTurnus, FrozenSet<Person> People);
