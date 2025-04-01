using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ametrin.Serialization;
using TTC.Console;
using TTC.Core.Rules;
using TTC.Core.Serialization;

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
};

var ruleType = typeof(Rule);
var ruleTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(assembly => assembly.GetTypes())
    .Where(type => type.IsSubclassOf(ruleType));

jsonOptions.Converters.Add(new RuleJsonConverter(ruleTypes));
jsonOptions.Converters.Add(new PersonJsonConverter());
jsonOptions.Converters.Add(new FrozenSetJsonConverter<Person>());
jsonOptions.Converters.Add(new EnumJsonConverter<Day>());

var jsonOptionsBinder = new JsonOptionsBinder(jsonOptions);

var seedOption = new Option<Random>("--seed", description: "The inital seed for the generator (same seed same output)", getDefaultValue: static () => Random.Shared);
seedOption.AddAlias("-s");

var outputOption = new Option<string?>("--output", description: "The output file path (absolute or relative to input)", getDefaultValue: static () => null);
outputOption.AddAlias("-o");

var rulesOption = new Option<string>("--rules", description: "The directory path to the rule configs", getDefaultValue: static () => "Rules");
rulesOption.AddAlias("-r");

var inputArgument = new Argument<string>("input");

var root = new RootCommand("Time Table Creator");

var generateKurseCommand = new Command("generate-kurse", description: "Given a json list of all subjects, generate a json list of randomly assigned kurse");
generateKurseCommand.AddArgument(inputArgument);
generateKurseCommand.AddOption(outputOption);
generateKurseCommand.AddOption(seedOption);
generateKurseCommand.SetHandler(GenerateCoursesCommand.Run, inputArgument, seedOption, jsonOptionsBinder);
root.AddCommand(generateKurseCommand);

var generateTimeTableCommand = new Command("generate", description: "Given a json list of all kurse, generate a json time table");
generateTimeTableCommand.AddArgument(inputArgument);
generateTimeTableCommand.AddOption(outputOption);
generateTimeTableCommand.AddOption(seedOption);
generateTimeTableCommand.AddOption(rulesOption);
generateTimeTableCommand.SetHandler(static (inputPath, random, outputPath, rulesPath, jsonOptions) =>
{
    var input = new FileInfo(inputPath);
    var output = input.Directory!.File(outputPath ?? $"{input.NameWithoutExtension()}_timetable.json");
    using var stream = input.OpenRead();
    var kurse = JsonSerializer.Deserialize<IEnumerable<Course>>(stream, jsonOptions) ?? throw new NullReferenceException();

    var rules = Directory.EnumerateFiles(rulesPath, "*.json").Select(f => JsonExtensions.ReadFromJsonFile<Rule>(f, jsonOptions).OrThrow());

    var wave = new PlanningWave(kurse, rules)
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
    jsonOptions.Converters.Add(new ReducedKursJsonConverter(wave.Courses));

    JsonExtensions.WriteToJsonFile(outputData, output, jsonOptions);

}, inputArgument, seedOption, outputOption, rulesOption, jsonOptionsBinder);
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

var exportDefaultRulesCommand = new Command("export-default-rules");
exportDefaultRulesCommand.AddOption(outputOption);
exportDefaultRulesCommand.SetHandler(static (outputPath, jsonOptions) =>
{
    var output = new DirectoryInfo(outputPath ?? "Rules");
    output.CreateIfNotExists();

    IEnumerable<Rule> rules = [
        new ModifyTimeSlots([0.6f, 1, 1, 1, 1, 0.6f, 0.1f, 0.05f]) { Name = "weight_time_slots" },
        new AvoidSameCourseTwiceADay(0.1f) { Name = "avoid_kurs_more_than_once_per_day" },
        new ReduceLessionsPerDay(5, 0.01f) { Name = "reduce_lessions_per_day" },
        new NoCoursesOnDays([Day.ASonntag, Day.BFreitag, Day.BSamstag, Day.BSonntag]) { Name = "no_lessions_on_days" },
        new RequireOneLessionPerPersonPerSlot() { Name = "require_one_lession_per_person_per_slot" },
    ];

    foreach (var rule in rules)
    {
        JsonExtensions.WriteToJsonFile(rule, output.File($"{rule.Name}.json"), jsonOptions);
    }
}, outputOption, jsonOptionsBinder);
root.AddCommand(exportDefaultRulesCommand);

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

file record EventsInSlot(Day Day, int Slot, List<Course> Kurse);

file class JsonOptionsBinder(JsonSerializerOptions options) : BinderBase<JsonSerializerOptions>
{
    public JsonSerializerOptions Options { get; } = options;

    protected override JsonSerializerOptions GetBoundValue(BindingContext context) => Options;
}
