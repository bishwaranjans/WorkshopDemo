// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

Console.WriteLine("Semantic Functions");
Console.WriteLine("------------------");

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

// Import the OrchestratorPlugin from the plugins directory.
var orchestratorPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestratorPlugin");

/*
// Get the GetIntent function from the OrchestratorPlugin and run it
var result = await orchestratorPlugin["GetIntent"]
     .InvokeAsync("I want to send an email to the marketing team celebrating their recent milestone.");

*/

var summarizationPlugin = kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "SummarizePlugin");

// Create a new context and set the input, history, and options variables.
var context = kernel.CreateNewContext();
context["input"] = "Yes";
context["history"] = @"Bot: How can I help you?
User: My team just hit a major milestone and I would like to send them a message to congratulate them.
Bot:Would you like to send an email?";
context["options"] = "SendEmail, ReadEmail, SendMeeting, RsvpToMeeting, SendChat";

// Run the Summarize function with the context.
var result = await orchestratorPlugin["GetIntent"].InvokeAsync(context);

Console.WriteLine(result);
