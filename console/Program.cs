using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ametrin.Serialization;
using TTC.Console;
using TTC.Core.Rules;


var jsonOptions = new JsonSerializerOptions()
{
    WriteIndented = true,
};

jsonOptions.Converters.Add(new PersonJsonConverter());
jsonOptions.Converters.Add(new FrozenSetJsonConverter<Person>());
jsonOptions.Converters.Add(new EnumJsonConverter<Day>());

var jsonOptionsBinder = new JsonOptionsBinder(jsonOptions);

var seedOption = new Option<Random>("--seed", description: "The inital seed for the generator (same seed same output)", getDefaultValue: static () => Random.Shared);
seedOption.AddAlias("-s");

var outputOption = new Option<string?>("--output", description: "The output file path (absolute or relative to input)", getDefaultValue: static () => null);
outputOption.AddAlias("-o");

var inputArgument = new Argument<string>("input");

var root = new RootCommand("Time Table Creator");
root.AddGlobalOption(seedOption);

var generateKurseCommand = new Command("generate-kurse", description: "Given a json list of all subjects, generate a json list of randomly assigned kurse");
generateKurseCommand.AddArgument(inputArgument);
generateKurseCommand.AddOption(outputOption);
generateKurseCommand.SetHandler(GenerateKurseCommand.Run, inputArgument, seedOption, jsonOptionsBinder);
root.AddCommand(generateKurseCommand);

var generateTimeTableCommand = new Command("generate", description: "Given a json list of all kurse, generate a json time table");
generateTimeTableCommand.AddArgument(inputArgument);
generateTimeTableCommand.AddOption(outputOption);
generateTimeTableCommand.SetHandler(static (inputPath, random, outputPath, jsonOptions) =>
{
    var input = new FileInfo(inputPath);
    var output = input.Directory!.File(outputPath ?? $"{input.NameWithoutExtension()}_timetable.json");
    using var stream = input.OpenRead();
    var kurse = JsonSerializer.Deserialize<IEnumerable<Kurs>>(stream, jsonOptions) ?? throw new NullReferenceException();

    var wave = new PlanningWave(kurse,
    [
        new ModifyTimeSlots([0.5f, 1, 1, 1, 1, 0.6f, 0.1f, 0.05f]),
        new AvoidSameKursTwiceADay(0.1f),
        new ReduceLessionsPerDay(5, 0.01f),
        new NoLessionsOnDays([Day.ASonntag, Day.BFreitag, Day.BSamstag, Day.BSonntag]),
        new RequireOneLessionPerPersonPerSlot(),
    ])
    {
        Random = random
    };

    do
    {
        wave.ApplyRules();
    }
    while (wave.CollapsNext());

    if (!wave.Validate())
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to generate time table! Try again");
        Console.ResetColor();

        return;
    }

    var outputData = new List<EventsInSlot>(wave.DayCount * wave.SlotsPerDay);

    foreach (var day in ..wave.DayCount)
    {
        foreach (var slot in ..wave.SlotsPerDay)
        {
            if (wave.FinalPlan[slot, day].Count > 0)
            {
                outputData.Add(new EventsInSlot((Day)day, slot, wave.FinalPlan[slot, day]));            
            }
        }
    }

    jsonOptions = new(jsonOptions);
    jsonOptions.Converters.Add(new ReducedKursJsonConverter(wave.Kurse));

    JsonExtensions.WriteToJsonFile(outputData, output, jsonOptions);

}, inputArgument, seedOption, outputOption, jsonOptionsBinder);
root.AddCommand(generateTimeTableCommand);

var debugCommand = new Command("debug");
debugCommand.AddOption(outputOption);
debugCommand.SetHandler(static (random, outputPath, jsonOptions) =>
{
    if (outputPath is null)
    {
        return;
    }

    var kurse = TestHelper.GenerateBigTestSet(random).ToImmutableArray();
    JsonExtensions.WriteToJsonFile(kurse, outputPath, jsonOptions);
}, seedOption, outputOption, jsonOptionsBinder);
root.AddCommand(debugCommand);

root.Invoke([.. args]);

// var validCount = 0;
// foreach (var i in ..50)
// {
//     wave.Reset();
//     wave.ApplyRules();

//     while (wave.CollapsNext())
//     {
//         wave.ApplyRules();
//     }

//     if (wave.Validate())
//     {
//         validCount++;
//     }
// }

// Console.WriteLine(validCount);

file record EventsInSlot(Day Day, int Slot, List<Kurs> Kurse);

file class JsonOptionsBinder(JsonSerializerOptions options) : BinderBase<JsonSerializerOptions>
{
    public JsonSerializerOptions Options { get; } = options;

    protected override JsonSerializerOptions GetBoundValue(BindingContext context) => Options;
}
