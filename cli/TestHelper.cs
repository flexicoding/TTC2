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
            new Subject("GK5", 5, [new("Weinbrenner")], [.. students.GetRandomElements(2, random)]),
            new Subject("Eth5", 5, [new("Egner")], [.. students.GetRandomElements(9, random)]),
            new Subject("Rel5", 5, [new("Droesch")], [.. students.GetRandomElements(2, random)]),
            new Subject("Wir5", 5, [new("Novak")], [.. students.GetRandomElements(4, random)]),
            new Subject("Eng5", 5, [new("Weber")], [.. students.GetRandomElements(6, random)]),
            new Subject("Lat5", 5, [new("Kutscherauer")], [.. students.GetRandomElements(4, random)]),
            new Subject("Fra5", 5, [new("Kutscherauer")], [.. students.GetRandomElements(4, random)]),
            new Subject("BK5", 5, [new("Loosing")], [.. students.GetRandomElements(3, random)]),
            new Subject("Mu5", 5, [new("Schäfer")], [.. students.GetRandomElements(6, random)]),
            new Subject("SP5", 5, [new("Engel")], [.. students.GetRandomElements(5, random)]),

            new Subject("Mat3", 3, [new("Maier"), new("Weißer")], [.. students.GetRandomElements(20, random)]),
            new Subject("Deu3", 3, [new("Droysen"), new("Knöpfler-Seitz"), new("Schwarz"), new("Hieber"), new("Heese")], [.. students.GetRandomElements(50, random)]),
            new Subject("Phy3", 3, [new("Wedemaier")], [.. students.GetRandomElements(12, random)]),
            new Subject("Che3", 3, [new("Schönborn")], [.. students.GetRandomElements(10, random)]),
            new Subject("Bio3", 3, [new("Akst")], [.. students.GetRandomElements(15, random)]),
            new Subject("Ges3", 3, [new("Heese"), new("Egner"), new("Sarbacher")], [.. students.GetRandomElements(55, random)]),
            new Subject("GK3", 3, [new("Akst")], [.. students.GetRandomElements(50, random)]),
            new Subject("Eth3", 3, [new("Weißer"), new("Knöpfler-Seitz")], [.. students.GetRandomElements(15, random)]),
            new Subject("Rel3", 3, [new("Droesch")], [.. students.GetRandomElements(8, random)]),
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
        const int PERSONS = 52;
        var m51 = Course("5M1", 5, ["Fischer", "Amelie", "Annabelle", "Catelyn", "Clara", "Felix H", "Georgy", "Helen", "Hendrik", "Jaron", "Joshua", "Linyi", "Luana", "Marie", "Matilda", "Michael", "Onyx", "Pia", "Richard", "Lotta", "Tina", "Sofiia"]);
        var m52 = Course("5M2", 5, ["Maier", "Jonathan", "Alexander", "Felix G", "Josef", "Lorenz", "Luka", "Moana", "Muhammed", "Violet", "William", "Eliah", "Fenja", "Lea", "Mara", "Serhii", "Lennard"]);
        var m31 = Course("3m1", 3, ["Maier", "Anna", "Annabel", "Bruno", "Emma S", "Emma W", "Johanna", "Miriana", "Nola", "Elin", "Tabitha", "Juliane", "Katharina", "Lilly", "Marie-M", "Lisa"]);
        MustContain(PERSONS + 2, m51, m52, m31);

        var d51 = Course("5D1", 5, ["Schwamm", "Bruno", "Anna", "Catelyn", "Emma W", "Lotta", "Juliane", "Katharina", "Lilly", "Marie-M"]);
        var d31 = Course("3d1", 3, ["Hieber", "Amelie", "Annabel", "Felix G", "Helen", "Johanna", "Josef", "Joshua", "Linyi", "Lorenz", "Matilda", "Michael", "Miriana", "Moana", "Muhammed", "Lisa"]);
        var d32 = Course("3d2", 3, ["Droysen", "Jonathan", "Alexander", "Annabelle", "Clara", "Emma S", "Felix H", "Jaron", "Luana", "Luka", "Marie", "Elin", "Tina", "Eliah"]);
        var d33 = Course("3d3", 3, ["Knöpfler-Seitz", "Hendrik", "Sofiia", "Pia", "Richard", "Serhii", "Tabitha", "Violet", "William", "Fenja", "Mara", "Nola", "Onyx", "Georgy", "Lea", "Lennard"]);
        MustContain(PERSONS + 4, d51, d31, d32, d33);

        var py51 = Course("5Ph1", 5, ["Brökelmann", "Jonathan", "Mara", "Fenja", "Luana", "Katharina", "Felix G", "Felix H", "Josef", "Alexander", "Joshua", "Tina", "Matilda", "Serhii", "Linyi"]);
        var py31 = Course("3ph1", 3, ["Wedemayer", "Jaron", "Richard", "Pia", "Helen", "Sofiia"]);

        var ch51 = Course("5Ch1", 5, ["Tsalastra", "Clara", "Hendrik", "Marie", "Onyx", "Pia", "Richard", "William", "Lennard"]);
        var ch52 = Course("5Ch2", 5, ["Richter", "Felix G", "Josef", "Muhammed", "Luka", "Linyi", "Annabelle", "Georgy", "Elin", "Jaron", "Mara", "Catelyn", "Felix H", "Tina", "Alexander"]);
        var ch31 = Course("3ch1", 3, ["Schönborn"]);

        var b51 = Course("5B1", 5, ["Akst", "Johanna", "Georgy", "Luka", "Catelyn"]);
        var b31 = Course("3b1", 3, ["Akst"]);

        var g51 = Course("5G1", 3, ["Heese", "Bruno", "Emma W", "Violet"]);
        var g21 = Course("2g1", 2, ["Heese", "Bruno", "Emma W", "Violet", "Jonathan", "Lorenz", "Hendrik", "Amelie", "Michael", "Pia", "Marie-M", "Lisa", "Lennard"]);
        var g22 = Course("2g2", 2, ["Egner", "Felix G", "Josef", "Muhammed", "Luka", "Georgy", "Elin", "Serhii", "Jaron", "Lotta", "Marie", "Mara", "William", "Helen", "Anna", "Catelyn", "Tina", "Sofiia", "Richard", "Onyx", "Alexander", "Clara"]);
        var g23 = Course("2g3", 2, ["Sarbacher"]);
        MustContain(PERSONS + 3, g21, g22, g23);

        var gk51 = Course("5GK1", 3, ["Weinbrenner", "Lilly", "Lotta"]);
        var gk21 = Course("2gk1", 2, ["Akst", "Felix G", "Amelie", "Josef", "Muhammed", "Luka", "Linyi", "Annabelle", "Georgy", "Joshua", "Eliah", "Emma S", "Lorenz", "Mara", "William", "Catelyn", "Felix H", "Tina", "Hendrik", "Richard", "Onyx", "Alexander", "Pia"]);
        var gk22 = Course("2gk2", 2, ["Akst", "Nola", "Miriana", "Michael", "Luana", "Juliane", "Johanna", "Tabitha", "Marie-M", "Katharina", "Moana", "Marie", "Emma W", "Lisa", "Fenja", "Helen", "Bruno", "Jonathan", "Matilda", "Violet", "Lea", "Clara"]);
        var gk24 = Course("2gk4", 2, ["Weinbrenner", "Lilly", "Lotta", "Annabel", "Anna", "Jaron"]);

        var eth51 = Course("5Eth1", 5, ["Egner", "Pia", "Amelie", "Michael", "Marie-M", "Lorenz", "Lisa", "Helen", "Hendrik", "Jonathan"]);
        var eth21 = Course("2eth1", 2, ["Weißer", "Moana", "Elin", "Bruno", "Lennard", "Joshua", "Georgy", "Felix H", "Annabelle", "Nola", "Muhammed", "Luana", "Eliah", "Emma S", "Felix G"]);
        var eth22 = Course("2eth2", 2, ["Knöpfler-Seitz", "Josef", "Luka", "Linyi", "Juliane", "Serhii", "Mara", "Anna", "Tina", "Sofiia", "Violet", "Lea"]);

        var rel51 = Course("5Rel1", 5, ["Droesch", "Johanna", "Tabitha"]);
        var rel21 = Course("2rel1", 2, ["Droesch", "Johanna", "Tabitha", "Emma W", "Catelyn", "Alexander", "Katharina", "Matilda", "Miriana", "Fenja"]);
        var rel22 = Course("2rel2", 2, ["Key-Häring", "Lilly", "Jaron", "Lotta", "Annabel", "Marie", "William", "Richard", "Onyx", "Clara"]);
        MustContain(PERSONS + 5, eth51, eth21, eth22, rel21, rel22);

        var wi51 = Course("5Wi1", 5, ["Novak", "Elin", "Serhii", "Lennard", "Sofiia"]);
        MustContain(PERSONS + 2, gk21, gk22, gk24, wi51);

        var e51 = Course("5E1", 5, ["Weber", "Amelie", "Anna", "Annabel", "Clara", "Eliah", "Emma S", "Helen", "Violet", "Jaron", "Juliane", "Katharina", "Michael", "Moana", "Lotta", "Sofiia", "Tabitha"]);
        var e31 = Course("3e1", 3, ["Benz", "Pia", "Felix G", "Muhammed", "Linyi", "Elin", "Luana", "Serhii", "Marie", "Lorenz", "Mara", "Bruno", "Felix H", "Tina", "Hendrik", "Richard", "Alexander", "Josef"]);
        var e32 = Course("3e2", 3, ["Kohler", "Nola", "Luka", "Miriana", "Annabelle", "Georgy", "Joshua", "Johanna", "Marie-M", "Emma W", "William", "Lisa", "Lennard", "Catelyn", "Onyx", "Jonathan", "Matilda", "Lea"]);
        MustContain(PERSONS + 3 - 2, e51, e31, e32); // - Lilly - Fenja

        var l51 = Course("5L1", 2, ["Kutscherauer", "Elin", "Johanna", "Marie", "Emma W", "Fenja"]);
        var l31 = Course("3l1", 3, ["Kutscherauer", "Miriana", "Elin", "Johanna", "Tabitha", "Marie", "Emma W", "Fenja", "Helen", "Jonathan", "Violet"]);

        var bk51 = Course("5BK1", 5, ["Losing", "Emma S", "Miriana", "Juliane"]);
        var bk21 = Course("2bk1", 2, ["Waidosch", "Jonathan", "Lorenz", "Bruno", "Nola", "Emma W", "Lennard", "Eliah", "Amelie", "Onyx", "Violet", "Lotta", "Sofiia"]);
        var bk22 = Course("2bk2", 2, ["Waidosch", "Jaron", "Annabelle", "Anna", "Josef", "Matilda", "Muhammed", "Katharina", "Mara"]);
        MustContain(21 + 1, bk21, bk22);

        var mu51 = Course("5Mu1", 5, ["Schäfer", "Joshua", "Lea", "Eliah", "Luana"]);
        var mu21 = Course("2mu1", 2, ["Schäfer", "Luka", "Linyi", "Lilly", "Annabelle", "Georgy", "Serhii", "Marie-M", "William", "Lisa", "Catelyn", "Tina", "Hendrik", "Richard"]);
        var mu22 = Course("2mu2", 2, ["Schäfer", "Felix H", "Alexander", "Clara", "Moana", "Helen", "Michael", "Fenja", "Annabel", "Marie", "Johanna", "Tabitha"]);

        var sp51 = Course("5Sp1", 3, ["Engel", "Nola", "Richard", "Onyx", "William"]);
        var sp21 = Course("2sp1", 2, ["Weiß"]);
        var sp22 = Course("2sp2", 2, ["Engel", "Pia", "Nola", "Luka", "Miriana", "Linyi", "Annabelle", "Luana", "Emma S", "Marie-M", "Katharina", "Annabel", "Lorenz", "William", "Lisa", "Catelyn", "Felix H", "Hendrik", "Richard", "Onyx", "Jonathan"]);
        var sp23 = Course("2sp3", 2, ["Schwamm", "Felix G", "Lilly", "Elin", "Johanna", "Clara", "Serhii", "Tabitha", "Moana", "Mara", "Bruno", "Tina", "Sofiia", "Lea"]);
        MustContain(PERSONS + 3, sp21, sp22, sp23);

        var mva21 = Course("2mva1", 2, ["Fischer", "Josef", "Muhammed", "Linyi", "Joshua", "Serhii", "Marie", "Fenja", "Felix H", "Tina", "Sofiia", "Matilda", "Alexander"]);

        var inf21 = Course("2inf1", 2, ["Häcker", "Jonathan", "Jaron", "Lennard", "Felix H", "Muhammed", "Emma S"]);

        var f51 = new Course("5f1", 2, ["Schubart", "Lea", "Marie-M", "Moana"]);
        var f31 = new Course("3f1", 3, ["Schubart", "Lea", "Marie-M", "Moana", "Nola", "Lilly", "Annabel", "Michael"]);

        ImmutableArray<Course> courses = [
            m51, m52, m31,
            d51, d31, d32, d33,
            py51, py31,
            ch51, ch52, ch31,
            b51, b31,
            g51, g21, g22, g23,
            gk51, gk21, gk22,
            eth51, eth21, eth22,
            rel51, rel21,
            wi51,
            e51, e31, e32,
            l51, l31,
            bk51, bk21, bk22,
            mu51, mu21, mu22,
            sp51, sp21, sp22, sp23,
            mva21,
            inf21,
        ];

        var all = courses.SelectMany(c => c.People).CountBy(p => p.ID).Where(p => p.Value > 4).Select(p => p.Key).ToImmutableArray();

        MustContainAll(m51, m52, m31);
        MustContainAll(d51, d31, d32, d33);
        MustContainAll(g21, g22, g23);
        MustContainAll(gk21, gk22, gk24, wi51);
        MustContainAll(eth51, eth21, eth22, rel21, rel22);
        MustContainAll(e51, e31, e32);
        MustContainAll(sp21, sp22, sp23);

        return courses;

        void MustContainAll(params IEnumerable<Course> courses)
        {
            var missing = all.Except(courses.SelectMany(c => c.People).Select(p => p.ID));
            if (missing.Any())
            {
                Console.WriteLine($"{string.Join(", ", courses.Select(c => c.Slug))} misses {string.Join(", ", missing)}");
            }
        }

        static void MustContain(int expected, params IEnumerable<Course> courses)
        {
            var real = courses.SelectMany(c => c.People).Distinct().Count();
            if (real != expected)
            {
                Console.WriteLine($"{real} instead of {expected} in {string.Join(", ", courses.Select(c => c.Slug))}");
            }
        }

        static Course Course(string slug, int lpt, IEnumerable<string> people)
            => new(slug, lpt, people.Select(s => new Person(s)).ToFrozenSet());
    }
}
