using TTC.Core;
using TTC.Core.Rules;

IEnumerable<Kurs> kurse = [
    new Kurs("Ethik 5", "Eth5", 5),
    new Kurs("Physik 5", "Phy5", 5),
    new Kurs("Mathe 5", "Mat5", 5),
    new Kurs("Latein 3", "Lat3", 3),
    new Kurs("Deutsch 3", "Deu3", 3),
    new Kurs("Geographie 3", "Geo3", 3),
    new Kurs("Informatik 2", "Inf2", 2),
    new Kurs("Kunst 2", "BK2", 2),
    new Kurs("Englisch 3", "Eng3", 3),
    new Kurs("Sport 2", "Sp3", 2),
    new Kurs("Geschichte 2", "Ges3", 2),
];

var wave = new PlanningWave(kurse.SelectMany(kurs => kurs.CreateLessions()),
[
    new LessionsAvoidFirstAndLast(0.5f),
    new AvoidSameKursOnADay(0.1f),
    new PreventMoreThan5LessionsPerDay(),
]);

wave.ApplyRules();
Console.WriteLine(wave.ToString(0));

while (wave.CollapsNext())
{
    wave.ApplyRules();
}

Console.WriteLine(wave.ToString(1));
