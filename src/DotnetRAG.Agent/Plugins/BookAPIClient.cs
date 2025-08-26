using DotnetRAG.Books.Actions.SearchBooks.Contracts;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace DotnetRAG.Agent.Plugins;

public class BookAPIClient
{
    private readonly HttpClient _httpClient;

    public BookAPIClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(BookAPIClient));
    }

    [KernelFunction(nameof(SearchBooksAsync))]
    [Description("Searches for books based on a query with the specified top number of results")]
    public async Task<SearchBooksResponse> SearchBooksAsync(string query, int top, CancellationToken cancellationToken)
    {
        var request = new SearchBooksRequest
        {
            Query = query,
            Top = top
        };

        var requestJson = JsonSerializer.Serialize(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5555/api/books/search")
        {
            Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")
        };

        var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
        httpResponse.EnsureSuccessStatusCode();

        var response = await httpResponse.Content.ReadFromJsonAsync<SearchBooksResponse>(cancellationToken: cancellationToken);

        if (response == null)
        {
            throw new InvalidOperationException($"Failed to deserialize the response to {nameof(SearchBooksResponse)}.");
        }

        return response;
    }
}
