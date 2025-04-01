using System.CommandLine;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ametrin.Serialization;
using TTC.Cli;
using TTC.Core.Rules;
using TTC.Core.Serialization;

var jsonHelper = new JsonHelper
{
    Options = new()
    {
        WriteIndented = true,
    }
};

var ruleType = typeof(Rule);
var ruleTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(assembly => assembly.GetTypes())
    .Where(type => type.IsSubclassOf(ruleType));

jsonHelper.Options.Converters.Add(new RuleJsonConverter(ruleTypes));
jsonHelper.Options.Converters.Add(new PersonJsonConverter());
jsonHelper.Options.Converters.Add(new FrozenSetJsonConverter<Person>());
jsonHelper.Options.Converters.Add(new EnumJsonConverter<Day>());

var jsonHelperBinder = new JsonHelperBinder(jsonHelper);

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
generateKurseCommand.SetHandler(GenerateCoursesCommand.Run, inputArgument, seedOption, jsonHelperBinder);
root.AddCommand(generateKurseCommand);

var generateTimeTableCommand = new Command("generate", description: "Given a json list of all kurse, generate a json time table");
generateTimeTableCommand.AddArgument(inputArgument);
generateTimeTableCommand.AddOption(outputOption);
generateTimeTableCommand.AddOption(seedOption);
generateTimeTableCommand.AddOption(rulesOption);
generateTimeTableCommand.SetHandler(GenerateTimeTableCommand.Run,inputArgument, seedOption, outputOption, rulesOption, jsonHelperBinder);
root.AddCommand(generateTimeTableCommand);

var debugCommand = new Command("debug");
debugCommand.AddOption(outputOption);
debugCommand.SetHandler(static (random, outputPath, jsonHelper) =>
{
    if (outputPath is null)
    {
        return;
    }

    var kurse = TestHelper.GenerateBigTestSet(random).ToImmutableArray();
    JsonExtensions.WriteToJsonFile(kurse, outputPath, jsonHelper.Options);
}, seedOption, outputOption, jsonHelperBinder);
root.AddCommand(debugCommand);

var exportDefaultRulesCommand = new Command("export-default-rules");
exportDefaultRulesCommand.AddOption(outputOption);
exportDefaultRulesCommand.SetHandler(static (outputPath, jsonHelper) =>
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
        JsonExtensions.WriteToJsonFile(rule, output.File($"{rule.Name}.json"), jsonHelper.Options);
    }
}, outputOption, jsonHelperBinder);
root.AddCommand(exportDefaultRulesCommand);

root.Invoke([.. args]);
