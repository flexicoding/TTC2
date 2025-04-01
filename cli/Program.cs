using System.CommandLine;
using System.Text.Json;
using TTC.Cli;
using TTC.Core.Serialization;

var jsonHelper = new JsonHelper
{
    Options = new(JsonSerializerOptions.Default)
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

var generateCoursesCommand = new Command("generate-kurse", description: "Given a json list of all subjects, generate a json list of randomly assigned kurse");
generateCoursesCommand.AddArgument(inputArgument);
generateCoursesCommand.AddOption(outputOption);
generateCoursesCommand.AddOption(seedOption);
generateCoursesCommand.SetHandler(GenerateCoursesCommand.Run, inputArgument, seedOption, outputOption, jsonHelperBinder);
root.AddCommand(generateCoursesCommand);

var generateTimeTableCommand = new Command("generate", description: "Given a json list of all kurse, generate a json time table");
generateTimeTableCommand.AddArgument(inputArgument);
generateTimeTableCommand.AddOption(outputOption);
generateTimeTableCommand.AddOption(seedOption);
generateTimeTableCommand.AddOption(rulesOption);
generateTimeTableCommand.SetHandler(GenerateTimeTableCommand.Run,inputArgument, seedOption, outputOption, rulesOption, jsonHelperBinder);
root.AddCommand(generateTimeTableCommand);

var exportDefaultRulesCommand = new Command("export-default-rules");
exportDefaultRulesCommand.AddOption(outputOption);
exportDefaultRulesCommand.SetHandler(ExportDefaultRulesCommand.Run, outputOption, jsonHelperBinder);
root.AddCommand(exportDefaultRulesCommand);

var debugCommand = new Command("debug");
debugCommand.AddOption(outputOption);
debugCommand.SetHandler(static (random, outputPath, jsonHelper) =>
{
    if (outputPath is null)
    {
        Console.WriteLine("output path required");
        return;
    }

    var kurse = TestHelper.GenerateBigTestSet(random).ToArray();
    jsonHelper.WriteCollection(kurse, new(outputPath));
}, seedOption, outputOption, jsonHelperBinder);
root.AddCommand(debugCommand);

root.Invoke([.. args]);
