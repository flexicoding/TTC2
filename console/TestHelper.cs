
namespace TTC.Console;

internal static class TestHelper
{
    public static IEnumerable<Kurs> GenerateSmallTestSet()
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
            new Kurs("Eth5", 5, [egner, jonathan]),
            new Kurs("Phy5", 5, [brökelmann, jonathan, felix]),
            new Kurs("Mat5", 5, [fischer, jaron, jonathan, felix]),
            new Kurs("Eng5", 5, [weber, jaron]),
            new Kurs("Spt5", 5, [engel]),
            new Kurs("Che3", 5, [egner]),
            new Kurs("Phy3", 5, [brökelmann, jaron]),
            new Kurs("Lat3", 3, [kutscherauer, jonathan]),
            new Kurs("Deu3", 3, [weber, jonathan, jaron, felix]),
            new Kurs("Geo3", 3, [kutscherauer, jaron, felix]),
            new Kurs("Inf2", 2, [hacker, jaron, jonathan, felix]),
            new Kurs("BK2", 2, [hacker, jaron, felix]),
            new Kurs("Eng3", 3, [fischer, felix, jonathan]),
            new Kurs("Sp3", 2, [engel, jaron, felix, jonathan]),
            new Kurs("Ges3", 2, [egner, jonathan, felix]),
        ];
    }

    public static IEnumerable<Kurs> GenerateBigTestSet(Random? random = null)
    {
        random ??= Random.Shared;

        var people = Enumerable.Range(0, 200).Select(static i => new Person($"Person {i}")).ToArray();

        return [
            new Kurs("Mat5", 5, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Kurs("Deu5", 5, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Phy5", 5, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Kurs("Che5", 5, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Bio5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Geo5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Ges5", 5, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Kurs("Eng5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Lat5", 5, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Kurs("Eth5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Rel5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Sp5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("BK5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Inf5", 2, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Mat3", 3, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Kurs("Deu3", 3, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Kurs("Phy3", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Che3", 5, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Kurs("Bio3", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Geo3", 3, [.. people.GetRandomElements(random.Next(20+5, 25+5), random)]),
            new Kurs("Ges3", 2, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Eng3", 3, [.. people.GetRandomElements(random.Next(20+5, 25+5), random)]),
            new Kurs("Lat3", 3, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Eth3", 3, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Rel3", 3, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Inf2", 2, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Kurs("BK2", 2, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Kurs("Sp2", 2, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
        ];
    }

    public static IEnumerable<Kurs> GetRealTestSet()
    {
        IEnumerable<Subject> subjects = [
            new Subject("Mat5", 5, [new("Maier"), new("Fischer")], []),
        ];

        return [];
    }
}
