using DotnetRAG.Agent.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Reflection;

var hostLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
                   throw new InvalidOperationException("Cannot determine assembly location.");

var configuration = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(hostLocation, "appsettings.json"), optional: false)
    .Build();

var agentConfig = configuration
    .GetSection("AzureAI")
    .GetSection("Agent");

var kernel = Kernel
    .CreateBuilder()
    .AddAzureOpenAIChatCompletion(agentConfig["model"], agentConfig["endpoint"], agentConfig["apiKey"])
    .Build();

var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddSingleton<BookAPIClient>()
    .BuildServiceProvider();

var bookApiClient = serviceProvider.GetRequiredService<BookAPIClient>();

kernel.Plugins.AddFromObject(bookApiClient, nameof(BookAPIClient));

ChatHistory chatHistory = [];

var agentRole = @$"You are BookWorm, a helpful agent that assists users in finding books of interest.
    Ask brief clarifying questions about preferences (genres, authors, themes, mood).
    Recommend books returned from the {nameof(BookAPIClient)} plugin.";

chatHistory.AddSystemMessage(agentRole);

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var settings = new OpenAIPromptExecutionSettings
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

var introPrompt = "Hello, please introduce yourself.";
chatHistory.AddUserMessage(introPrompt);

var introResponse = await chatService.GetChatMessageContentAsync(
    chatHistory,
    executionSettings: settings,
    kernel: kernel);

Console.WriteLine($"Book Worm: {introResponse.Content}\n");
chatHistory.AddAssistantMessage(introResponse.Content ?? string.Empty);

while (true)
{
    Console.Write("You: ");

    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    chatHistory.AddUserMessage(input);

    var response = await chatService.GetChatMessageContentAsync(
        chatHistory,
        executionSettings: settings,
        kernel: kernel);

    Console.WriteLine();
    Console.WriteLine($"Book Worm: {response.Content}\n");

    chatHistory.AddAssistantMessage(response.Content ?? string.Empty);
}