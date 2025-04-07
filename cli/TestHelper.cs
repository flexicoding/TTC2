using Ametrin.Guards;

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

    public static IEnumerable<Course> GenerateRealisticTestSet(Random random)
    {
        var students = Enumerable.Range(0, 60).Select(i => new Person($"Person {i}")).ToImmutableArray();

        IEnumerable<Subject> subjects = [
            new Subject("Mat5", 5, [new("Maier"), new("Fischer")], [.. students.GetRandomElements(40, random)]),
            new Subject("Deu5", 5, [new("Schwamm")], [.. students.GetRandomElements(10, random)]),
            new Subject("Phy5", 5, [new("Brökelmann")], [.. students.GetRandomElements(13, random)]),
            new Subject("Che5", 5, [new("Richter"), new("Tsalastra"), new("Schönborn")], [.. students.GetRandomElements(35, random)]),
            new Subject("Bio5", 5, [new("Akst"), new("Schönborn")], [.. students.GetRandomElements(10, random)]),
            new Subject("Ges5", 5, [new("Heese")], [.. students.GetRandomElements(3, random)]),
            new Subject("Eth5", 5, [new("Egner")], [.. students.GetRandomElements(9, random)]),
            new Subject("Rel5", 5, [new("Droesch")], [.. students.GetRandomElements(2, random)]),
            new Subject("GK5", 5, [new("Weinbrenner")], [.. students.GetRandomElements(2, random)]),
            new Subject("Wir5", 5, [new("Novak")], [.. students.GetRandomElements(4, random)]),
            new Subject("Eng5", 5, [new("Weber")], [.. students.GetRandomElements(6, random)]),
            new Subject("Lat5", 5, [new("Kutscherauer")], [.. students.GetRandomElements(4, random)]),
            new Subject("Fra5", 5, [new("Kutscherauer")], [.. students.GetRandomElements(4, random)]),
            new Subject("BK5", 5, [new("Loosing")], [.. students.GetRandomElements(3, random)]),
            new Subject("Mu5", 5, [new("Schäfer")], [.. students.GetRandomElements(6, random)]),
            new Subject("SP5", 5, [new("Engel")], [.. students.GetRandomElements(5, random)]),

            new Subject("Mat3", 3, [new("Maier"), new("Weißer")], [.. students.GetRandomElements(20, random)]),
            new Subject("Deu3", 3, [new("Droysen"), new("Knöpflerseitz"), new("Schwarz"), new("Hieber"), new("Heese")], [.. students.GetRandomElements(50, random)]),
            new Subject("Phy3", 3, [new("Wedemaier")], [.. students.GetRandomElements(12, random)]),
            new Subject("Che3", 3, [new("Schönborn")], [.. students.GetRandomElements(10, random)]),
            new Subject("Bio3", 3, [new("Akst")], [.. students.GetRandomElements(15, random)]),
            new Subject("Ges3", 3, [new("Heese"), new("Egner"), new("Sarbacher")], [.. students.GetRandomElements(55, random)]),
            new Subject("Eth3", 3, [new("Weißer"), new("Knöpflerseitz")], [.. students.GetRandomElements(15, random)]),
            new Subject("Rel3", 3, [new("Droesch")], [.. students.GetRandomElements(8, random)]),
            new Subject("GK3", 3, [new("Akst")], [.. students.GetRandomElements(50, random)]),
            new Subject("Eng3", 3, [new("Benz"), new("Kohler")], [.. students.GetRandomElements(50, random)]),
            new Subject("Lat3", 3, [new("Kutscherauer")], [.. students.GetRandomElements(12, random)]),
            new Subject("Fra3", 3, [new("Kutscherauer")], [.. students.GetRandomElements(12, random)]),
            new Subject("BK2", 2, [new("Waidosch")], [.. students.GetRandomElements(30, random)]),
            new Subject("Mu2", 2, [new("Schäfer")], [.. students.GetRandomElements(30, random)]),
            new Subject("Sp2", 2, [new("Engel"), new("Schwamm"), new("Weiß")], [.. students.GetRandomElements(50, random)]),

        ];

        return subjects.SelectMany(s => s.DivideIntoCourses(20, random));
    }

    public static IEnumerable<Course> GetRealTestSet()
    {
        var m51 = new Course("5M1", 5, ["Fischer", "Amelie", "Annabelle", "Catelyn", "Clara", "Felix H", "Georgy", "Helen", "Hendrik", "Jaron", "Joshua", "Linyi", "Luana", "Marie", "Matilda", "Michael", "Onyx", "Pia", "Richard", "Lotta", "Tina", "Sofiia"]);
        var m52 = new Course("5M2", 5, ["Maier", "Jonathan", "Alexander", "Felix G", "Josef", "Lorenz", "Luka", "Moana", "Muhammed", "Violet", "William", "Eliah", "Fenja", "Lea", "Mara"]);
        var m31 = new Course("3m1", 3, ["Maier", "Anna", "Annabel", "Bruno", "Emma S", "Emma W", "Johanna", "Miriana", "Nola", "Elin", "Tabitha", "Juliane", "Kathie", "Lilly", "Marie-M"]);
        MustContainAll(m51, m52, m31);

        var d51 = new Course("5D1", 5, ["Schwamm", "Bruno", "Anna", "Catelyn", "Emma W", "Lotta", "Juliane", "Kathie", "Lilly", "Marie-M"]);
        var d31 = new Course("3d1", 3, ["Hieber", "Amelie", "Annabel", "Felix G", "Helen", "Johanna", "Josef", "Joshua", "Linyi", "Lorenz", "Mathilda", "Michael", "Miriana", "Moana", "Muhammed"]);
        var d32 = new Course("3d2", 3, ["Droysen", "Jonathan", "Alexander", "Annabelle", "Clara", "Emma S", "Felix H", "Jaron", "Luana", "Luka", "Marie", "Elin", "Tina", "Eliah"]);
        var d33 = new Course("3d3", 3, ["Knöpfi", "Hendrik", "Sofia", "Pia", "Richard", "Serhii", "Tabitha", "Violet", "William", "Fenja", "Mara", "Nola", "Onyx"]);
        MustContainAll(d51, d31, d32, d33);


        var py51 = new Course("5Ph1", 5, ["Brökelmann", "Jonathan", "Mara", "Fenja", "Luana", "Kathie", "Felix G", "Felix H", "Josef", "Alex", "Joshua", "Tina", "Mathilda", "Serhii", "Linyi"]);
        var py31 = new Course("3ph1", 3, ["Wedemayer", "Jaron", "Richard", "Pia", "Helen", "Sofiia"]);


        var ch51 = new Course("5Ch1", 5, ["Tsalastra"]);
        var ch52 = new Course("5Ch2", 5, ["Richter"]);
        var ch31 = new Course("3ch1", 3, ["Schönborn"]);

        var b51 = new Course("5B1", 5, ["Akst"]);
        var b31 = new Course("3b1", 3, ["Akst"]);

        return [
            m51, m52, m31,
            d51, d31, d32, d33,
            py51, py31,
            ch51, ch52, ch31,
        ];

        static void MustContainAll(params IEnumerable<Course> courses)
        {
            const int PERSONS = 49;
            ArgumentOutOfRangeException.ThrowIfNotEqual(courses.Sum(c => c.People.Count), PERSONS);
            ArgumentOutOfRangeException.ThrowIfNotEqual(courses.Skip(1).Aggregate(courses.First().People.AsEnumerable(), (u, c) => u.UnionBy(c.People, static p => p.ID)).Count(), 0);
        }
    }
}
