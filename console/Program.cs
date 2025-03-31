using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;
using System.Text.Json;
using Ametrin.Serialization;
using TTC.Console;
using TTC.Core;
using TTC.Core.Rules;


var jsonOptions = new JsonSerializerOptions()
{
    WriteIndented = true,
};

jsonOptions.Converters.Add(new PersonJsonConverter());

var jsonOptionsBinder = new JsonOptionsBinder(jsonOptions);

var seedOption = new Option<Random>("--seed", description: "The inital seed for the generator (same seed same output)", getDefaultValue: static () => Random.Shared);
seedOption.AddAlias("-s");

var outputOption = new Option<int>("--output", description: "The output file path (absolute or relative to input)");
outputOption.AddAlias("-o");

var fileArgument = new Argument<string>("file");

var root = new RootCommand("Time Table Creator");
root.AddGlobalOption(seedOption);

var generateKurseCommand = new Command("generate-kurse", description: "Given a json list of all subjects, generate a json list of randomly assigned kurse");

generateKurseCommand.AddArgument(fileArgument);
generateKurseCommand.SetHandler(GenerateKurseCommand.Run, fileArgument, seedOption, jsonOptionsBinder);

var generateTimeTableCommand = new Command("generate", description: "Given a json list of all kurse, generate a json time table");

root.AddCommand(generateKurseCommand);
root.AddCommand(generateTimeTableCommand);

// root.Invoke([..args, "-h"]);

// // var random = new Random(420);
// var random = Random.Shared;

// var wave = new PlanningWave(kurse,
// [
//     new ModifyTimeSlots([0.6f, 1, 1, 1, 1, 0.6f, 0.1f, 0.05f]),
//     new AvoidSameKursTwiceADay(0.1f),
//     new ReduceLessionsPerDay(5, 0.01f),
//     new NoLessionsOnDays([Day.ASonntag, Day.BFreitag, Day.BSamstag, Day.BSonntag]),
//     new RequireOneLessionPerPersonPerSlot(),
// ])
// {
//     // Random = new Random(69)
// };

// Console.WriteLine(wave.People.Length);

// wave.ApplyRules();
// Console.WriteLine(wave.ToString(0));

// while (wave.CollapsNext())
// {
//     wave.ApplyRules();
// }

// wave.Validate();

// Console.WriteLine(wave.ToString(1));


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

file class JsonOptionsBinder(JsonSerializerOptions options) : BinderBase<JsonSerializerOptions>
{
    public JsonSerializerOptions Options { get; } = options;

    protected override JsonSerializerOptions GetBoundValue(BindingContext bindingContext) => Options;
}