using Azure;
using Azure.AI.Inference;
using DotnetRAG.Books.Tools;

namespace DotnetRAG.Tools.AzureAI;

public class AzureAIEmbeddings(string model, string endpoint, string apiKey) : IEmbeddings
{
    private readonly EmbeddingsClient _embeddingsClient = new(new Uri(endpoint), new AzureKeyCredential(apiKey));

    public async Task<float[]> GenerateAsync(string value, CancellationToken cancellationToken)
    {
        var options = new EmbeddingsOptions([value])
        {
            Model = model
        };

        var result = await _embeddingsClient.EmbedAsync(options, cancellationToken: cancellationToken);
        var vector = result?.Value?.Data?.FirstOrDefault()?.Embedding?.ToObjectFromJson<float[]>();

        if (vector == null || vector.Length == 0)
        {
            throw new InvalidOperationException("Failed to generate embedding.");
        }

        return vector;
    }
}