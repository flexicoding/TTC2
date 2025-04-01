using System.CommandLine.Binding;
using TTC.Core.Serialization;

namespace TTC.Cli;


internal class JsonHelperBinder(JsonHelper helper) : BinderBase<JsonHelper>
{
    public JsonHelper Helper { get; } = helper;

    protected override JsonHelper GetBoundValue(BindingContext context) => Helper;
}