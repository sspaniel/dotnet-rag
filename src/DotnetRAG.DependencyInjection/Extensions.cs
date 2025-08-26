using DotnetRAG.Books;
using DotnetRAG.Books.Actions.AddBook.Orchestration;
using DotnetRAG.Books.Actions.EmbedBook.Orchestration;
using DotnetRAG.Books.Actions.SearchBooks.Orchestration;
using DotnetRAG.Books.Tools;
using DotnetRAG.Tools.AzureAI;
using DotnetRAG.Tools.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Connectors.Redis;

namespace DotnetRAG.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddBookService(this IServiceCollection services, IConfiguration configuration)
    {
        AddTools(services, configuration);
        AddActions(services);
        services.AddScoped<IBookService, BookService>();
        return services;
    }

    private static void AddTools(IServiceCollection services, IConfiguration configuration)
    {
        var embeddingsConfig = configuration
            .GetSection("AzureAI")
            .GetSection("Embeddings");

        services.AddSingleton<IEmbeddings>(serviceProvider =>
        {
            var model = embeddingsConfig["model"];
            var endpoint = embeddingsConfig["endpoint"];
            var apiKey = embeddingsConfig["apiKey"];
            
            return new AzureAIEmbeddings(model, endpoint, apiKey);
        });

        var redisConfig = configuration
            .GetSection("Redis");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig["ConnectionString"];
        });

        var redisVectorStoreOptions = new RedisVectorStoreOptions
        {
            StorageType = RedisStorageType.Json,
        };

        services.AddRedisVectorStore(redisConfig["ConnectionString"], redisVectorStoreOptions);

        services.AddScoped<IBookRepository, RedisBookRepository>();
    }

    private static void AddActions(IServiceCollection services)
    {
        services.AddScoped<IAddBookOrchestrator, AddBookOrchestrator>();
        services.AddScoped<IEmbedBookOrchestrator, EmbedBookOrchestrator>();
        services.AddScoped<ISearchBookOrchestrator, SearchBookOrchestrator>();
    }
}