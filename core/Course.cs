namespace TTC.Core;

public sealed record Course(string Slug, int LessionsPerTurnus, FrozenSet<Person> People) : IEquatable<Course>
{
    public bool Equals(Course? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null && Slug == other.Slug;
    }

    public override int GetHashCode() => Slug.GetHashCode();
}
