using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Plugins;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(0)
        .AddDebug();
});
var logger = loggerFactory.CreateLogger<Kernel>();

var kernelSettings = KernelSettings.LoadSettings();
IKernel kernel = new KernelBuilder()
    .WithCompletionService(kernelSettings)
    .WithLogger(logger)
    .Build();

var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

// Import the semantic functions
kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestratorPlugin");
kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "SummarizePlugin");

// Import the native functions
var mathPlugin = kernel.ImportSkill(new MathPlugin(), "MathPlugin");
/*

// Run the Sqrt function
var result1 = await mathPlugin["Sqrt"].InvokeAsync("64");
Console.WriteLine(result1);

// Run the Add function with multiple inputs
var context = kernel.CreateNewContext();
context["input"] = "3";
context["number2"] = "7";
var result2 = await mathPlugin["Add"].InvokeAsync(context);
Console.WriteLine(result2);
*/

var orchestratorPlugin = kernel.ImportSkill(new OrchestratorPlugin(kernel), "OrchestratorPlugin");
// Make a request that runs the Sqrt function
var result1 = await orchestratorPlugin["RouteRequest"].InvokeAsync("What is the square root of 634?");
Console.WriteLine(result1);

// Make a request that runs the Add function
var result2 = await orchestratorPlugin["RouteRequest"].InvokeAsync("What is 42 plus 1513?");
Console.WriteLine(result2);

