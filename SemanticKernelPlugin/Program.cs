using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(0)
        .AddDebug();
});
var logger = loggerFactory.CreateLogger<Kernel>();

// Create a kernel
var kernelSettings = KernelSettings.LoadSettings();
IKernel kernel = new KernelBuilder()
    .WithCompletionService(kernelSettings)
    .WithLogger(logger)
    .Build();

// Add the math plugin using the plugin manifest URL
const string pluginManifestUrl = "http://localhost:7071/.well-known/ai-plugin.json";
var mathPlugin = await kernel.ImportChatGptPluginSkillFromUrlAsync("MathPlugin", new Uri(pluginManifestUrl));

// Create a stepwise planner and invoke it
var planner = new StepwisePlanner(kernel);
var question = "I have $2130.23. How much money would I have if it grew by 5.25% and after I bought a $10 latte from Starbucks?";
var plan = planner.CreatePlan(question);

var jsonString = JsonConvert.SerializeObject(
           plan, Formatting.Indented,
           new JsonConverter[] { new StringEnumConverter() });
Console.WriteLine(jsonString);

var result = await plan.InvokeAsync(kernel.CreateNewContext());

// Print the results
Console.WriteLine("Result: " + result);

// Print details about the plan
if (result.Variables.TryGetValue("stepCount", out string? stepCount))
{
    Console.WriteLine("Steps Taken: " + stepCount);
}
if (result.Variables.TryGetValue("skillCount", out string? skillCount))
{
    Console.WriteLine("Skills Used: " + skillCount);
}
