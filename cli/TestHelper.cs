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
}
