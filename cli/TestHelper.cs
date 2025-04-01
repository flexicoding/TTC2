namespace TTC.Cli;

internal static class TestHelper
{
    public static IEnumerable<Course> GenerateSmallTestSet()
    {
        var jaron = new Person("jaron");
        var jonathan = new Person("jonathan");
        var felix = new Person("felix");
        var hacker = new Person("häcker");
        var weber = new Person("weber");
        var fischer = new Person("fischer");
        var engel = new Person("engel");
        var egner = new Person("egner");
        var brökelmann = new Person("brökelmann");
        var kutscherauer = new Person("kutscherauer");

        return [
            new Course("Eth5", 5, [egner, jonathan]),
            new Course("Phy5", 5, [brökelmann, jonathan, felix]),
            new Course("Mat5", 5, [fischer, jaron, jonathan, felix]),
            new Course("Eng5", 5, [weber, jaron]),
            new Course("Spt5", 5, [engel]),
            new Course("Che3", 5, [egner]),
            new Course("Phy3", 5, [brökelmann, jaron]),
            new Course("Lat3", 3, [kutscherauer, jonathan]),
            new Course("Deu3", 3, [weber, jonathan, jaron, felix]),
            new Course("Geo3", 3, [kutscherauer, jaron, felix]),
            new Course("Inf2", 2, [hacker, jaron, jonathan, felix]),
            new Course("BK2", 2, [hacker, jaron, felix]),
            new Course("Eng3", 3, [fischer, felix, jonathan]),
            new Course("Sp3", 2, [engel, jaron, felix, jonathan]),
            new Course("Ges3", 2, [egner, jonathan, felix]),
        ];
    }

    public static IEnumerable<Course> GenerateBigTestSet(Random? random = null)
    {
        random ??= Random.Shared;

        var people = Enumerable.Range(0, 200).Select(static i => new Person($"Person {i}")).ToArray();

        return [
            new Course("Mat5", 5, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Course("Deu5", 5, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Course("Phy5", 5, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Course("Che5", 5, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Course("Bio5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Geo5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Course("Ges5", 5, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Course("Eng5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Lat5", 5, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Course("Eth5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Rel5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Course("Sp5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("BK5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Course("Inf5", 2, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Course("Mat3", 3, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Course("Deu3", 3, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Course("Phy3", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Che3", 5, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Course("Bio3", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Geo3", 3, [.. people.GetRandomElements(random.Next(20+5, 25+5), random)]),
            new Course("Ges3", 2, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Course("Eng3", 3, [.. people.GetRandomElements(random.Next(20+5, 25+5), random)]),
            new Course("Lat3", 3, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Eth3", 3, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Course("Rel3", 3, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Course("Inf2", 2, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Course("BK2", 2, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Course("Sp2", 2, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
        ];
    }

    public static IEnumerable<Course> GetRealTestSet()
    {
        IEnumerable<Subject> subjects = [
            new Subject("Mat5", 5, [new("Maier"), new("Fischer")], []),
        ];

        return [];
    }
}
