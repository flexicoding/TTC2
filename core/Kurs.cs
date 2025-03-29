namespace TTC.Core;

public sealed record Kurs(string Name, string Slug, int LessionCount)
{
    public string Slug { get; init; } = Slug.Length <= 4 ? Slug.PadRight(4) : throw new ArgumentException();
    public IEnumerable<Lession> CreateLessions() => Enumerable.Range(0, LessionCount).Select(_ => new Lession(this));
}
