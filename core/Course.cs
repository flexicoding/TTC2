namespace TTC.Core;

public sealed record Course(string Slug, int LessionsPerTurnus, FrozenSet<Person> People)
{
    public Course(string slug, int lpt, IEnumerable<string> people) 
        : this(slug, lpt, people.Select(s => new Person(s)).ToFrozenSet()) { }
}
