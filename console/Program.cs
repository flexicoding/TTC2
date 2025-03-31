using System.Text.Json;
using TTC.Core;
using TTC.Core.Rules;

// var random = new Random(420);
var random = Random.Shared;

var jsonOptions = new JsonSerializerOptions()
{
    WriteIndented = true,
};

jsonOptions.Converters.Add(new PersonJsonConverter());

// var jaron = new Person("jaron");
// var jonathan = new Person("jonathan");
// var felix = new Person("felix");
// var hacker = new Person("häcker");
// var weber = new Person("weber");
// var fischer = new Person("fischer");
// var engel = new Person("engel");
// var egner = new Person("egner");
// var brökelmann = new Person("brökelmann");
// var kutscherauer = new Person("kutscherauer");


// IEnumerable<Kurs> kurse = [
//     new Kurs("Ethik 5", "Eth5", 5, [egner, jonathan]),
//     new Kurs("Physik 5", "Phy5", 5, [brökelmann, jonathan, felix]),
//     new Kurs("Mathe 5", "Mat5", 5, [fischer, jaron, jonathan, felix]),
//     new Kurs("Englisch 5", "Eng5", 5, [weber, jaron]),
//     new Kurs("Sport 5", "Spt5", 5, [engel]),
//     new Kurs("Chemie 3", "Che3", 5, [egner]),
//     new Kurs("Physik 3", "Phy3", 5, [brökelmann, jaron]),
//     new Kurs("Latein 3", "Lat3", 3, [kutscherauer, jonathan]),
//     new Kurs("Deutsch 3", "Deu3", 3, [weber, jonathan, jaron, felix]),
//     new Kurs("Geographie 3", "Geo3", 3, [kutscherauer, jaron, felix]),
//     new Kurs("Informatik 2", "Inf2", 2, [hacker, jaron, jonathan, felix]),
//     new Kurs("Kunst 2", "BK2", 2, [hacker, jaron, felix]),
//     new Kurs("Englisch 3", "Eng3", 3, [fischer, felix, jonathan]),
//     new Kurs("Sport 2", "Sp3", 2, [engel, jaron, felix, jonathan]),
//     new Kurs("Geschichte 3", "Ges3", 2, [egner, jonathan, felix]),
// ];

var people = Enumerable.Range(0, 200).Select(static i => new Person($"Person {i}")).ToArray();

IEnumerable<Kurs> kurse = [
    new Kurs("Mathe 5", "Mat5", 5, [.. people.GetRandomElements(random.Next(15+10, 20+10), random)]),
    new Kurs("Deutsch 5", "Deu5", 5, [.. people.GetRandomElements(random.Next(10+10, 15+10), random)]),
    new Kurs("Physik 5", "Phy5", 5, [.. people.GetRandomElements(random.Next(8+10, 12+10), random)]),
    new Kurs("Chemie 5", "Che5", 5, [.. people.GetRandomElements(random.Next(10+10, 15+10), random)]),
    new Kurs("Biologie 5", "Bio5", 5, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Geographie 5", "Geo5", 5, [.. people.GetRandomElements(random.Next(2+10, 6+10), random)]),
    new Kurs("Geschichte 5", "Ges5", 5, [.. people.GetRandomElements(random.Next(3+10, 8+10), random)]),
    new Kurs("Englisch 5", "Eng5", 5, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Latein 5", "Lat5", 5, [.. people.GetRandomElements(random.Next(3+10, 8+10), random)]),
    new Kurs("Ethik 5", "Eth5", 5, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Religion 5", "Rel5", 5, [.. people.GetRandomElements(random.Next(2+10, 6+10), random)]),
    new Kurs("Sport 5", "Sp5", 5, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Kunst 5", "BK5", 5, [.. people.GetRandomElements(random.Next(2+10, 6+10), random)]),
    new Kurs("Informatik 5", "Inf5", 2, [.. people.GetRandomElements(random.Next(2+10, 6+10), random)]),
    new Kurs("Mathe 3", "Mat3", 3, [.. people.GetRandomElements(random.Next(15+10, 20+10), random)]),
    new Kurs("Deutsch 3", "Deu3", 3, [.. people.GetRandomElements(random.Next(15+10, 20+10), random)]),
    new Kurs("Physik 3", "Phy3", 5, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Chemie 3", "Che3", 5, [.. people.GetRandomElements(random.Next(8+10, 12+10), random)]),
    new Kurs("Biologie 3", "Bio3", 5, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Geographie 3", "Geo3", 3, [.. people.GetRandomElements(random.Next(20+10, 25+10), random)]),
    new Kurs("Geschichte 3", "Ges3", 2, [.. people.GetRandomElements(random.Next(10+10, 15+10), random)]),
    new Kurs("Englisch 3", "Eng3", 3, [.. people.GetRandomElements(random.Next(20+10, 25+10), random)]),
    new Kurs("Latein 3", "Lat3", 3, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Ethik 3", "Eth3", 3, [.. people.GetRandomElements(random.Next(10+10, 15+10), random)]),
    new Kurs("Religion 3", "Rel3", 3, [.. people.GetRandomElements(random.Next(5+10, 10+10), random)]),
    new Kurs("Informatik 2", "Inf2", 2, [.. people.GetRandomElements(random.Next(3+10, 8+10), random)]),
    new Kurs("Kunst 2", "BK2", 2, [.. people.GetRandomElements(random.Next(8+10, 12+10), random)]),
    new Kurs("Sport 2", "Sp2", 2, [.. people.GetRandomElements(random.Next(15+10, 20+10), random)]),
];

var wave = new PlanningWave(kurse,
[
    new ModifyTimeSlots([0.6f, 1, 1, 1, 1, 0.6f, 0.1f, 0.05f]),
    new AvoidSameKursTwiceADay(0.1f),
    new ReduceLessionsPerDay(5, 0.01f),
    new NoLessionsOnDays([Day.ASonntag, Day.BFreitag, Day.BSamstag, Day.BSonntag]),
    new RequireOneLessionPerPersonPerSlot(),
])
{
    // Random = new Random(69)
};

Console.WriteLine(wave.People.Length);

wave.ApplyRules();
Console.WriteLine(wave.ToString(0));

while (wave.CollapsNext())
{
    wave.ApplyRules();
}

wave.Validate();

Console.WriteLine(wave.ToString(1));


var validCount = 0;
foreach (var i in ..50)
{
    wave.Reset();
    wave.ApplyRules();

    while (wave.CollapsNext())
    {
        wave.ApplyRules();
    }

    if (wave.Validate())
    {
        validCount++;
    }
}

Console.WriteLine(validCount);