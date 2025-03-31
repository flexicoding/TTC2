
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
            new Kurs("Ethik 5", "Eth5", 5, [egner, jonathan]),
            new Kurs("Physik 5", "Phy5", 5, [brökelmann, jonathan, felix]),
            new Kurs("Mathe 5", "Mat5", 5, [fischer, jaron, jonathan, felix]),
            new Kurs("Englisch 5", "Eng5", 5, [weber, jaron]),
            new Kurs("Sport 5", "Spt5", 5, [engel]),
            new Kurs("Chemie 3", "Che3", 5, [egner]),
            new Kurs("Physik 3", "Phy3", 5, [brökelmann, jaron]),
            new Kurs("Latein 3", "Lat3", 3, [kutscherauer, jonathan]),
            new Kurs("Deutsch 3", "Deu3", 3, [weber, jonathan, jaron, felix]),
            new Kurs("Geographie 3", "Geo3", 3, [kutscherauer, jaron, felix]),
            new Kurs("Informatik 2", "Inf2", 2, [hacker, jaron, jonathan, felix]),
            new Kurs("Kunst 2", "BK2", 2, [hacker, jaron, felix]),
            new Kurs("Englisch 3", "Eng3", 3, [fischer, felix, jonathan]),
            new Kurs("Sport 2", "Sp3", 2, [engel, jaron, felix, jonathan]),
            new Kurs("Geschichte 3", "Ges3", 2, [egner, jonathan, felix]),
        ];
    }

    public static IEnumerable<Kurs> GenerateBigTestSet(Random? random = null)
    {
        random ??= Random.Shared;

        var people = Enumerable.Range(0, 200).Select(static i => new Person($"Person {i}")).ToArray();

        return [
            new Kurs("Mathe 5", "Mat5", 5, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Kurs("Deutsch 5", "Deu5", 5, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Physik 5", "Phy5", 5, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Kurs("Chemie 5", "Che5", 5, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Biologie 5", "Bio5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Geographie 5", "Geo5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Geschichte 5", "Ges5", 5, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Kurs("Englisch 5", "Eng5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Latein 5", "Lat5", 5, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Kurs("Ethik 5", "Eth5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Religion 5", "Rel5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Sport 5", "Sp5", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Kunst 5", "BK5", 5, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Informatik 5", "Inf5", 2, [.. people.GetRandomElements(random.Next(2+5, 6+5), random)]),
            new Kurs("Mathe 3", "Mat3", 3, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Kurs("Deutsch 3", "Deu3", 3, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
            new Kurs("Physik 3", "Phy3", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Chemie 3", "Che3", 5, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Kurs("Biologie 3", "Bio3", 5, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Geographie 3", "Geo3", 3, [.. people.GetRandomElements(random.Next(20+5, 25+5), random)]),
            new Kurs("Geschichte 3", "Ges3", 2, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Englisch 3", "Eng3", 3, [.. people.GetRandomElements(random.Next(20+5, 25+5), random)]),
            new Kurs("Latein 3", "Lat3", 3, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Ethik 3", "Eth3", 3, [.. people.GetRandomElements(random.Next(10+5, 15+5), random)]),
            new Kurs("Religion 3", "Rel3", 3, [.. people.GetRandomElements(random.Next(5+5, 10+5), random)]),
            new Kurs("Informatik 2", "Inf2", 2, [.. people.GetRandomElements(random.Next(3+5, 8+5), random)]),
            new Kurs("Kunst 2", "BK2", 2, [.. people.GetRandomElements(random.Next(8+5, 12+5), random)]),
            new Kurs("Sport 2", "Sp2", 2, [.. people.GetRandomElements(random.Next(15+5, 20+5), random)]),
        ];
    }

    public static IEnumerable<Kurs> GetRealTestSet()
    {
        IEnumerable<Subject> subjects = [
            new Subject("Mathe 5", "Mat5", 5, [new("Maier"), new("Fischer")], []),
        ];

        return [];
    }
}
